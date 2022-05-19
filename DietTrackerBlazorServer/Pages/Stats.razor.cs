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
using DietTrackerBlazorServer.Services;

namespace DietTrackerBlazorServer.Pages
{
    public partial class Stats : DTPageBase
    {
        class ChartFilterConfiguration
        {
            public int Id;
            public string Name;
            public TimeSpan ChartSpan;
            public int IntervalCount;
        }


        [Inject]
        ICorrelationCalculator _CorrelationCalculator { get; set; }
        LineChart<ChartPoint> _LineChart { get; set; }

        LineChartOptions _LineChartOptions { get; set; } = new LineChartOptions
        {
            Scales = new ChartScales
            {
                X = new ChartAxis
                {
                    Type = "linear",
                    Ticks = new ChartAxisTicks
                    {
                        //Callback = (value, index, ticks) => $"{value * 10.0}"
                    }

                },
                Y = new ChartAxis
                {
                    BeginAtZero = true,
                    Max = 10.0,
                    Type = "linear"
                }
            },
        };
        List<ChartFilterConfiguration> _ChartFilterConfigurations { get; set; } = new List<ChartFilterConfiguration>
        {
            new ChartFilterConfiguration{Id = 0, Name = "6 hours", ChartSpan = new TimeSpan(0,6,0,0), IntervalCount = 12},
            new ChartFilterConfiguration{Id = 1, Name = "1 day", ChartSpan = new TimeSpan(1,0,0,0), IntervalCount = 12},
            new ChartFilterConfiguration{Id = 2, Name = "1 week", ChartSpan = new TimeSpan(7,0,0,0), IntervalCount = 12},
            new ChartFilterConfiguration{Id = 3, Name = "1 month", ChartSpan = new TimeSpan(28,0,0,0), IntervalCount = 12},
            new ChartFilterConfiguration{Id = 4, Name = "3 months", ChartSpan = new TimeSpan(84,0,0,0), IntervalCount = 12},
            new ChartFilterConfiguration{Id = 5, Name = "6 months", ChartSpan = new TimeSpan(168,0,0,0), IntervalCount = 12},
            new ChartFilterConfiguration{Id = 6, Name = "1 year", ChartSpan = new TimeSpan(336,0,0,0), IntervalCount = 12}
        };


        int _SelectedChartFilterConfigurationIndex { get; set; } = 1;
        ChartFilterConfiguration _SelectedChartFilterConfiguration => _ChartFilterConfigurations[_SelectedChartFilterConfigurationIndex];

        TimeSpan _FoodEffectWindow = TimeSpan.FromDays(1.0);

        Dictionary<FoodType, Dictionary<HealthMetric, double>> _Correlations { get; set; } = new Dictionary<FoodType, Dictionary<HealthMetric, double>>();


        struct ChartPoint
        {
            public object X { get; set; }
            public object Y { get; set; }
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            return base.SetParametersAsync(parameters);
        }

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            _Correlations = await _CorrelationCalculator.GetAllCorrelations();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                SetAppLoading(true);
                await UpdateChart();
                SetAppLoading(false);
            }
        }

        async Task UpdateChart()
        {
            var userId = await GetUserIdAsync();

            using (ApplicationDbContext dbContext = await _DbContextFactory.CreateDbContextAsync())
            {
                await _LineChart.Clear();

                DateTime chartEndTime = DateTime.Now;
                DateTime chartStartTime = chartEndTime - _SelectedChartFilterConfiguration.ChartSpan;

                var query = await dbContext.HealthDataPoints
                    .Where(e => e.ApplicationUserId == userId)
                    .Where(e => e.Date >= chartStartTime && e.Date < chartEndTime)
                    .Include(e => e.HealthMetric)
                    .AsNoTracking()
                    .ToListAsync();


                //Group by has issues being translated into a SQL query
                //so we execute the query first, causing the group by to be run on the client.
                var dataPointGroups = query.OrderBy(e => e.Date)
                                           .GroupBy(e => e.HealthMetricId);

                if (!dataPointGroups.Any())
                    await _SnackbarStack.PushAsync($"No data were found for the selected time span.", SnackbarColor.Warning);

                //List<string> labels = new List<string>();
                //for (int i = 0; i < chartConfiguration.IntervalCount; i++)
                //{
                //    labels.Add((chartStartTime + chartIntervalTimeSpan * i).ToString());
                //}

                List<LineChartDataset<ChartPoint>> datasets = new List<LineChartDataset<ChartPoint>>();

                //_BorderColors.Clear();
                foreach (var group in dataPointGroups)
                {
                    var dataset = new LineChartDataset<ChartPoint>();
                    dataset.Label = group.First().HealthMetric.Name;
                    dataset.BorderColor = (string)ChartColor.FromHtmlColorCode(group.First().HealthMetric.Color);
                    dataset.Data = new List<ChartPoint>();
                    dataset.Fill = false;
                    datasets.Add(dataset);

                    foreach (var datapoint in group)
                    {
                        dataset.Data.Add(new ChartPoint
                        {
                            X = DateTimeToChartPosition(datapoint.Date, chartEndTime, _SelectedChartFilterConfiguration),
                            Y = datapoint.Value
                        });
                    }
                }


                //_LineChartOptions.Scales.X.Min = DateTimeToChartPosition(chartStartTime, chartEndTime, _SelectedChartFilterConfiguration);
                //_LineChartOptions.Scales.X.Max = DateTimeToChartPosition(chartEndTime, chartEndTime, _SelectedChartFilterConfiguration);
                _LineChartOptions.Scales.X.Min = 0.0;
                _LineChartOptions.Scales.X.Max = 1.0;
                //await _LineChart.AddLabels("a", "b", "c", "d", "e", "f", "g");

                //Configure labels for chart ticks
                //List<string> labels = new List<string>();
                //for (int i = 0; i <= _SelectedChartFilterConfiguration.IntervalCount; i++)
                //{
                //    DateTime tickTime = chartStartTime + (_SelectedChartFilterConfiguration.ChartSpan / _SelectedChartFilterConfiguration.IntervalCount) * i;
                //    labels.Add($"{chartStartTime}");
                //}
                //await _LineChart.AddLabels(labels);
                //_LineChart.Options.Scales.X.Ticks.
                datasets.ForEach(async d => await _LineChart.AddDataSet(d));
                await _LineChart.Update();
            }


        }

        async Task OnFiltersApplied()
        {
            SetAppLoading(true);
            await UpdateChart();
            SetAppLoading(false);
        }


        double DateTimeToChartPosition(DateTime time, DateTime chartEndTime, ChartFilterConfiguration config)
        {
            TimeSpan chartIntervalTimeSpan = config.ChartSpan / config.IntervalCount;
            DateTime chartStartTime = chartEndTime - config.ChartSpan;

            return (time - chartStartTime) / (chartEndTime - chartStartTime);
        }






    }

}