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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DietTrackerBlazorServer.Pages
{
    public partial class Stats
    {
        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        UserManager<ApplicationUser> UserManager { get; set; }

        [Inject]
        IDbContextFactory<ApplicationDbContext> DbContextFactory { get; set; }

        [CascadingParameter]
        SnackbarStack SnackbarStack { get; set; }

        LineChart<double> LineChart { get; set; }
        List<string> BorderColors { get; set; } = new List<string>();

        LineChartOptions LineChartOptions { get; set; } = new LineChartOptions
        {
            Scales = new ChartScales
            {
                Y = new ChartAxis
                {
                    BeginAtZero = true,
                    Max = 10.0
                }
            },
            Animation = new ChartAnimation
            {
                Delay = 0,
                Duration = 150

            },
            Plugins = new ChartPlugins
            {
                Decimation = new ChartDecimation
                {
                    
                }
            }
        };


        int SelectedChartDisplayConfiguration { get; set; } = 1;
        //TimeSpan ChartTimeSpan { get; set; } = new TimeSpan(7, 0, 0, 0);
        //int ChartResolution { get; set; } = 10;

        //protected override async Task OnInitializedAsync()
        //{

        //}

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await UpdateChart();
            }
        }

        async Task UpdateChart()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userId = await UserManager.GetUserIdAsync(await UserManager.GetUserAsync(authState.User));

            using (ApplicationDbContext dbContext = await DbContextFactory.CreateDbContextAsync())
            {
                await LineChart.Clear();
                //LineChart.Options = LineChartOptions;

                ChartDisplayConfiguration chartConfiguration = ChartDisplayConfigurations[SelectedChartDisplayConfiguration];

                DateTime currentTime = DateTime.Now;
                //DateTime currentHour = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 0, 0)
                //    + new TimeSpan(1, 0, 0);
                //TimeSpan chartTickInterval = ChartTickIntervals[ChartTickIntervalIndex];
                TimeSpan chartIntervalTimeSpan = chartConfiguration.ChartSpan / chartConfiguration.IntervalCount;
                DateTime chartStartTime = DateTime.Now - chartConfiguration.ChartSpan;

                var query = await dbContext.HealthDataPoints
                    .Where(e => e.ApplicationUserId == userId)
                    .Where(e => e.Date >= chartStartTime && e.Date < currentTime)
                    .Include(e => e.HealthMetric)
                    .AsNoTracking()
                    .ToListAsync();

                //var dataPoints = await query //Execute query

                //Group by has issues being translated into a SQL query
                //so we execute the query first, causing the group by to be run on the client.
                var dataPointGroups = query.OrderBy(e => e.Date)
                                           .GroupBy(e => e.HealthMetricId);

                if (!dataPointGroups.Any())
                    await SnackbarStack.PushAsync($"No data were found for the selected time span.", SnackbarColor.Warning);

                List<string> labels = new List<string>();
                for (int i = 0; i < chartConfiguration.IntervalCount; i++)
                {
                    labels.Add((chartStartTime + chartIntervalTimeSpan * i).ToString());
                }

                List<LineChartDataset<double>> datasets = new List<LineChartDataset<double>>();

                BorderColors.Clear();
                foreach (var group in dataPointGroups)
                {
                    var dataset = new LineChartDataset<double>();
                    dataset.Label = group.First().HealthMetric.Name;
                    dataset.BorderColor = (string)ChartColor.FromHtmlColorCode(group.First().HealthMetric.Color);
                    dataset.Data = new List<double>();
                    dataset.Fill = false;

                    datasets.Add(dataset);
                    

                    for (int i = 0; i < chartConfiguration.IntervalCount; i++)
                    {
                        var dataPoints = group.ToList();
                        var tickTime = chartStartTime + i * chartIntervalTimeSpan;
                        dataset.Data.Add(Utilities.GetInterpolatedDataPointValue(dataPoints, tickTime));
                    }
                }

                //Important to add labels before data for correct display.
                await LineChart.AddLabels(labels.ToArray());
                for (int i = 0; i < datasets.Count; i++)
                {
                    //datasets[i].BorderColor = BorderColors[i];
                    await LineChart.AddDataSet(datasets[i]);
                }
                await LineChart.Update();
            }


        }

        async Task OnFiltersApplied()
        {
            await UpdateChart();
        }



        struct ChartDisplayConfiguration
        {
            public TimeSpan ChartSpan;
            public int IntervalCount;
        }

        List<ChartDisplayConfiguration> ChartDisplayConfigurations { get; set; } = new List<ChartDisplayConfiguration>
        {
            new ChartDisplayConfiguration{ChartSpan = new TimeSpan(0,6,0,0), IntervalCount = 12},
            new ChartDisplayConfiguration{ChartSpan = new TimeSpan(1,0,0,0), IntervalCount = 12},
            new ChartDisplayConfiguration{ChartSpan = new TimeSpan(7,0,0,0), IntervalCount = 12},
            new ChartDisplayConfiguration{ChartSpan = new TimeSpan(28,0,0,0), IntervalCount = 12},
            new ChartDisplayConfiguration{ChartSpan = new TimeSpan(84,0,0,0), IntervalCount = 12},
            new ChartDisplayConfiguration{ChartSpan = new TimeSpan(168,0,0,0), IntervalCount = 12},
            new ChartDisplayConfiguration{ChartSpan = new TimeSpan(336,0,0,0), IntervalCount = 12}
        };
    }
}