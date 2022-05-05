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
using DietTrackerBlazorServer.Data;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Blazorise.Snackbar;

namespace DietTrackerBlazorServer.Pages
{
    public partial class HealthMetrics : ComponentBase
    {
        [Inject]
        AuthenticationStateProvider _AuthenticationStateProvider { get; set; }
        [Inject]
        UserManager<ApplicationUser> _UserManager { get; set; }
        [Inject]
        IDbContextFactory<ApplicationDbContext> _DbContextFactory { get; set; }
        bool Loaded { get; set; }


        [CascadingParameter]
        SnackbarStack _SnackbarStack { get; set; }
        Modal _AddModal { get; set; }
        Modal _EditModal { get; set; }
        Modal _DeleteModal { get; set; }

        List<HealthMetric> _CurrentHealthMetrics { get; set; } = new List<HealthMetric>();
        HealthMetric _DataGridSelectedHealthMetric { get; set; }


        HealthMetric _SubjectHealthMetric { get; set; } = new HealthMetric();

        protected override async Task OnInitializedAsync()
        {
            await ReloadData();
        }

        protected async Task ReloadData()
        {
            Loaded = false;
            using (ApplicationDbContext dbContext = await _DbContextFactory.CreateDbContextAsync())
            {
                var authState = await _AuthenticationStateProvider.GetAuthenticationStateAsync();
                var userId = await _UserManager.GetUserIdAsync(await _UserManager.GetUserAsync(authState.User));

                _CurrentHealthMetrics = await dbContext.HealthMetrics
                    .Where(e => e.ApplicationUserId == userId)
                    .ToListAsync();

            }
            Loaded = true;
        }

        protected void OnAddButtonClicked()
        {
            _SubjectHealthMetric = new HealthMetric();
            _AddModal.Show();
        }

        protected void OnEditButtonClicked()
        {
            _SubjectHealthMetric = _DataGridSelectedHealthMetric.ShallowCopy();
            //SubjectHealthMetric.ApplicationUserId = DataGridSelectedHealthMetric.ApplicationUserId;
            //SubjectHealthMetric.Name = DataGridSelectedHealthMetric.Name;
            //SubjectHealthMetric.Description = DataGridSelectedHealthMetric.Description;

            _EditModal.Show();
        }

        protected void OnDeleteButtonClicked()
        {
            _SubjectHealthMetric = _DataGridSelectedHealthMetric;
            _DeleteModal.Show();
        }

        protected async Task OnValidSubmit_AddHealthMetric()
        {
            //Get current user id
            var authState = await _AuthenticationStateProvider.GetAuthenticationStateAsync();
            _SubjectHealthMetric.ApplicationUserId = await _UserManager.GetUserIdAsync(await _UserManager.GetUserAsync(authState.User));

            using (ApplicationDbContext dbContext = await _DbContextFactory.CreateDbContextAsync())
            {
                var query = dbContext.HealthMetrics
                        .Where(m => m.ApplicationUserId == _SubjectHealthMetric.ApplicationUserId)
                        .Where(m => m.Name == _SubjectHealthMetric.Name);

                if (await query.AnyAsync())
                {
                    await _SnackbarStack.PushAsync($"Failed: A health metric of the name \"{_SubjectHealthMetric.Name}\" already exists.", SnackbarColor.Danger);
                }
                else
                {
                    await dbContext.AddAsync(_SubjectHealthMetric);
                    if (await dbContext.SaveChangesAsync() > 0)
                    {
                        await _SnackbarStack.PushAsync($"Health metric added successfully.", SnackbarColor.Success);
                    }
                    else
                    {
                        await _SnackbarStack.PushAsync($"Failed to add health metric.", SnackbarColor.Danger);
                    }
                }

            }
            await _AddModal.Close(CloseReason.UserClosing);
            await ReloadData();
        }

        protected async Task OnValidSubmit_EditHealthMetric()
        {
            using (ApplicationDbContext dbContext = await _DbContextFactory.CreateDbContextAsync())
            {
                var query = dbContext.HealthMetrics
                        .Where(m => m.ApplicationUserId == _SubjectHealthMetric.ApplicationUserId)
                        .Where(m => m.Id != _SubjectHealthMetric.Id)
                        .Where(m => m.Name == _SubjectHealthMetric.Name);
                

                if (await query.AnyAsync())
                {
                    await _SnackbarStack.PushAsync($"Failed: A health metric of the name \"{_SubjectHealthMetric.Name}\" already exists.", SnackbarColor.Danger);
                }
                else
                {
                    dbContext.Update(_SubjectHealthMetric);
                    if (await dbContext.SaveChangesAsync() > 0)
                    {
                        await _SnackbarStack.PushAsync($"Health metric modified successfully.", SnackbarColor.Success);
                    }
                    else
                    {
                        await _SnackbarStack.PushAsync($"Failed to modify health metric.", SnackbarColor.Danger);
                    }

                }
            }
            await _EditModal.Close(CloseReason.UserClosing);
            await ReloadData();
        }

        protected async Task OnDeleteHealthMetricConfirmed()
        {
            using (ApplicationDbContext dbContext = await _DbContextFactory.CreateDbContextAsync())
            {
                var authState = await _AuthenticationStateProvider.GetAuthenticationStateAsync();
                var userId = await _UserManager.GetUserIdAsync(await _UserManager.GetUserAsync(authState.User));

                var associatedDataPoints = dbContext.HealthDataPoints
                    .Where(e => e.ApplicationUserId == userId)
                    .Where(e => e.HealthMetricId == _SubjectHealthMetric.Id);

                foreach (var dataPoint in associatedDataPoints)
                {
                    dbContext.Remove(dataPoint);
                }

                dbContext.Remove(_SubjectHealthMetric);
                if (await dbContext.SaveChangesAsync() > 0)
                {
                    await _SnackbarStack.PushAsync($"Health metric deleted successfully.", SnackbarColor.Success);
                }
                else
                {
                    await _SnackbarStack.PushAsync($"Failed to delete health metric.", SnackbarColor.Danger);
                }
            }
            _DataGridSelectedHealthMetric = null;
            await _DeleteModal.Close(CloseReason.UserClosing);
            await ReloadData();
        }

        


    }
}
