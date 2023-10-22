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
using DietTrackerBlazorServer.Areas.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DietTrackerBlazorServer.Pages
{
    public partial class AddHealthDataPoint : DTComponentBase
    {
        List<HealthMetric> HealthMetrics { get; set; } = new List<HealthMetric>();
        List<HealthDataPointDGItem> NewDataPoints = new List<HealthDataPointDGItem>();
        bool UseCurrentDate { get; set; } = true;
        DateTime SelectedDate { get; set; } = DateTime.Now;
        bool Loaded { get; set; } = false;


        protected override async Task OnInitializedAsync()
        {
            
            Loaded = false;
            string userId = await GetUserIdAsync();
            using (ApplicationDbContext dbContext = await _DbContextFactory.CreateDbContextAsync())
            {
                HealthMetrics = await dbContext.HealthMetrics.Where(e => e.ApplicationUserId == userId).AsNoTracking().ToListAsync();
            }

            NewDataPoints = new List<HealthDataPointDGItem>();
            foreach (var metric in HealthMetrics)
            {
                NewDataPoints.Add(new HealthDataPointDGItem { HealthMetric = metric, Include = true, Value = 5 });
            }

            Loaded = true;
            
            //table.
        }

        async Task OnSubmitButtonClicked()
        {
            
            string userId = await GetUserIdAsync();
            using (ApplicationDbContext dbContext = await _DbContextFactory.CreateDbContextAsync())
            {
                foreach (var dataPoint in NewDataPoints)
                {
                    if (dataPoint.Include)
                    {
                        HealthDataPoint newDataPoint = new HealthDataPoint { Date = UseCurrentDate ? DateTime.Now : SelectedDate, Value = dataPoint.Value, ApplicationUserId = userId, HealthMetricId = dataPoint.HealthMetric.Id };
                        dbContext.Add(newDataPoint);
                    }
                }

                if (await dbContext.SaveChangesAsync() > 0)
                {
                    //await _SnackbarStack.PushAsync($"Successfully added datapoints.", SnackbarColor.Success);
                }
                else
                {
                    //await _SnackbarStack.PushAsync($"Database failure.", SnackbarColor.Danger);
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