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
using DietTrackerBlazorServer.Shared;
using DietTrackerBlazorServer.Data;
using DietTrackerBlazorServer.Model;
using Blazorise;
using Blazorise.DataGrid;
using Blazorise.Snackbar;
using Blazorise.Charts;
using DietTrackerBlazorServer.Areas.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DietTrackerBlazorServer.Pages
{
    public partial class AddHealthDataPoint
    {
        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        UserManager<ApplicationUser> UserManager { get; set; }

        [Inject]
        IDbContextFactory<ApplicationDbContext> DbContextFactory { get; set; }

        [CascadingParameter]
        SnackbarStack SnackbarStack { get; set; }

        List<HealthMetric> HealthMetrics { get; set; } = new List<HealthMetric>();
        List<HealthDataPointDGItem> NewDataPoints = new List<HealthDataPointDGItem>();
        bool UseCustomDataPointDate { get; set; } = false;
        DateTime DataPointDate { get; set; } = DateTime.Now;
        bool Loaded { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            Loaded = false;
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            string userId = await UserManager.GetUserIdAsync(await UserManager.GetUserAsync(authState.User));
            using (ApplicationDbContext dbContext = await DbContextFactory.CreateDbContextAsync())
            {
                HealthMetrics = await dbContext.HealthMetrics.Where(e => e.ApplicationUserId == userId).AsNoTracking().ToListAsync();
            }

            NewDataPoints = new List<HealthDataPointDGItem>();
            foreach (var metric in HealthMetrics)
            {
                NewDataPoints.Add(new HealthDataPointDGItem{HealthMetric = metric, Include = true, Value = 5});
            }

            Loaded = true;
        }

        async Task OnSubmitButtonClicked()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            string userId = await UserManager.GetUserIdAsync(await UserManager.GetUserAsync(authState.User));
            using (ApplicationDbContext dbContext = await DbContextFactory.CreateDbContextAsync())
            {
                foreach (var dataPoint in NewDataPoints)
                {
                    if (dataPoint.Include)
                    {
                        HealthDataPoint newDataPoint = new HealthDataPoint{Date = UseCustomDataPointDate ? DataPointDate : DateTime.Now, Value = dataPoint.Value, ApplicationUserId = userId, HealthMetricId = dataPoint.HealthMetric.Id};
                        dbContext.Add(newDataPoint);
                    }
                }

                if (await dbContext.SaveChangesAsync() > 0)
                {
                    await SnackbarStack.PushAsync($"Successfully added datapoints.", SnackbarColor.Success);
                }
                else
                {
                    await SnackbarStack.PushAsync($"Database failure.", SnackbarColor.Danger);
                }
            }
        }

        class HealthDataPointDGItem
        {
            public int Value { get; set; }

            public bool Include { get; set; }

            public HealthMetric HealthMetric { get; set; }
        }
    }
}