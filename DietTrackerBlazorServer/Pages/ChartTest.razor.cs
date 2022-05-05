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
using DietTrackerBlazorServer.Pages;
using DietTrackerBlazorServer.Shared;
using DietTrackerBlazorServer.Data;
using DietTrackerBlazorServer.Model;
using Blazorise;
using Blazorise.DataGrid;
using Blazorise.Snackbar;
using Blazorise.Charts;

namespace DietTrackerBlazorServer.Pages
{
    public partial class ChartTest
    {
        LineChart<ChartPoint> Chart { get; set; }
        object ChartOptions { get; set; } = new
        {
            Scales = new ChartScales
            {
                X = new ChartAxis
                {
                    Display = true,
                    Min = 0,
                    Max = 5
                }
            }
        };


        public struct ChartPoint
        {
            public object X { get; set; }
            public object Y { get; set; }
        }

        List<string> BorderColors { get; set; } = new List<string>
        {
            ChartColor.FromRgba(255, 0, 0, 1f),
            ChartColor.FromRgba(0, 255, 0, 1f),
            ChartColor.FromRgba(0, 0, 255, 1f)
        };

        List<string> BackgroundColors { get; set; } = new List<string>
        {
            ChartColor.FromRgba(255, 0, 0, .2f),
            ChartColor.FromRgba(0, 255, 0, .2f),
            ChartColor.FromRgba(0, 0, 255, .2f)
        };

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                LineChartDataset<ChartPoint> dataset1 = new LineChartDataset<ChartPoint>
                {
                    Data = new List<ChartPoint>
                    {
                        new ChartPoint{X = 0.0, Y = 0.5},
                        new ChartPoint{X = 1.0, Y = 0.2},
                        new ChartPoint{X = 1.5, Y = 0.3},
                        new ChartPoint{X = 2.1, Y = 0.8},
                        new ChartPoint{X = 4.0, Y = 0.1},
                    },
                    BackgroundColor = BackgroundColors
                };



                //dataset1.BorderColor = ChartColor.FromRgba(255, 0, 0, 1f);
                //dataset1.BackgroundColor = ChartColor.FromRgba(255, 0, 0, .2f);
                LineChartDataset<ChartPoint> dataset2 = new LineChartDataset<ChartPoint>
                {
                    Data = new List<ChartPoint>
                    {
                        new ChartPoint{X = 0.0, Y = 0.9},
                        new ChartPoint{X = .5, Y = 0.6},
                        new ChartPoint{X = 1.8, Y = 0.2},
                        new ChartPoint{X = 3.6, Y = 0.3},
                        new ChartPoint{X = 3.9, Y = 0.5},
                    },
                    BackgroundColor = BackgroundColors
                };
                //dataset2.BorderColor = ChartColor.FromRgba(0, 255, 0, 1f);
                //dataset2.BackgroundColor = ChartColor.FromRgba(0, 255, 0, .2f);



                //string[] labels = new string[] { "a", "b", "c", "d", "e" };
                //await Chart.AddLabels(labels);

                await Chart.AddDataSet(dataset1);
                await Chart.AddDataSet(dataset2);
                //dataset1.x





                await Chart.Update();
            }
        }
    }
}