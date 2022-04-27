using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using DietTrackerBlazorServer;
using DietTrackerBlazorServer.Model;
using Blazorise;
//using Blazorise.Bootstrap;
using Blazorise.DataGrid;
using DietTrackerBlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Blazorise.Snackbar;

namespace DietTrackerBlazorServer.Pages
{
    public partial class HealthMetrics
    {
        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        protected UserManager<ApplicationUser> UserManager { get; set; }
        [Inject]
        protected IDbContextFactory<ApplicationDbContext> DbContextFactory { get; set; }
        protected bool Loaded { get; set; }

        [CascadingParameter]
        protected SnackbarStack SnackbarStack { get; set; }
        protected Modal AddModal { get; set; }
        protected Modal EditModal { get; set; }
        protected Modal DeleteModal { get; set; }

        protected List<HealthMetric> CurrentHealthMetrics { get; set; }
        public HealthMetric DataGridSelectedHealthMetric { get; set; }


        protected HealthMetric SubjectHealthMetric { get; set; } = new HealthMetric();

        protected void OnAddButtonClicked()
        {
            SubjectHealthMetric = new HealthMetric();
            AddModal.Show();
        }

        protected void OnEditButtonClicked()
        {
            SubjectHealthMetric = DataGridSelectedHealthMetric;
            EditModal.Show();
        }

        protected void OnDeleteButtonClicked()
        {
            SubjectHealthMetric = DataGridSelectedHealthMetric;
            DeleteModal.Show();
        }

        protected async Task OnValidSubmit_AddHealthMetric()
        {
            //Validation already done via aspnet EditForm

            //Get current user id
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            SubjectHealthMetric.ApplicationUserId = await UserManager.GetUserIdAsync(await UserManager.GetUserAsync(authState.User));
            

            using (ApplicationDbContext dbContext = DbContextFactory.CreateDbContext())
            {
                //Verify that a metric doesn't already exist with this name
                var query = from metric in dbContext.HealthMetrics
                            where metric.ApplicationUserId == SubjectHealthMetric.ApplicationUserId
                            && metric.Name == SubjectHealthMetric.Name
                            select metric;

                if (await query.AnyAsync())
                {
                    await SnackbarStack.PushAsync($"A health metric of the name \"{SubjectHealthMetric.Name}\" already exists.", SnackbarColor.Danger);
                    return;
                }
                else
                {
                    await dbContext.AddAsync(SubjectHealthMetric);
                    int numChanges = await dbContext.SaveChangesAsync();

                    if (numChanges > 0)
                    {
                        //Success
                        await SnackbarStack.PushAsync($"Added health metric \"{SubjectHealthMetric.Name}\" successfully.", SnackbarColor.Success);
                        await ReloadData();
                    }
                    else
                    {
                        throw new ApplicationException("Failed to add to database.");
                    }
                }
                await AddModal.Close(CloseReason.UserClosing);
            }
        }

        protected async Task OnValidSubmit_EditHealthMetric()
        {
            using (ApplicationDbContext dbContext = DbContextFactory.CreateDbContext())
            {
                var query = from metric in dbContext.HealthMetrics
                            where metric.ApplicationUserId == SubjectHealthMetric.ApplicationUserId
                            && metric.Name == SubjectHealthMetric.Name
                            select metric;

                if (query.Any())
                {
                    await SnackbarStack.PushAsync($"A health metric already exists with this name.", SnackbarColor.Danger);
                }
                else
                {
                    dbContext.Update(SubjectHealthMetric);
                    await dbContext.SaveChangesAsync();
                    await ReloadData();
                }
            }
            await EditModal.Close(CloseReason.UserClosing);
        }

        protected async Task OnDeleteHealthMetricConfirmed()
        {
            using (ApplicationDbContext dbContext = DbContextFactory.CreateDbContext())
            {
                dbContext.Remove(SubjectHealthMetric);
                await dbContext.SaveChangesAsync();
                await DeleteModal.Close(CloseReason.UserClosing);
                await ReloadData();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await ReloadData();
        }

        protected async Task ReloadData()
        {
            Loaded = false;
            using (ApplicationDbContext dbContext = DbContextFactory.CreateDbContext())
            {
                var query = from metric in dbContext.HealthMetrics
                            select metric;

                CurrentHealthMetrics = await query.ToListAsync();

            }
            Loaded = true;
        }
    }
}
