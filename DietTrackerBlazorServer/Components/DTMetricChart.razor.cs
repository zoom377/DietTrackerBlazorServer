using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;
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
using Blazorise.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;

namespace DietTrackerBlazorServer.Components
{

    public partial class DTMetricChart
    {
        [Parameter]
        public ViewSpanType ViewSpan { get; set; }


        protected LineChart<object> Chart { get; set; }
        protected ChartData<object> ChartData { get; set; }
        protected LineChartOptions ChartOptions { get; set; } = new()
        {
            //Scales = new()
            //{
            //    X = new()
            //    {
            //        Title = new() { Text = "Time" },
            //        Type = "Time",
            //        Time = new()
            //        {
            //            Unit = "day"
            //        }
            //    },
            //    Y = new()
            //    {
            //        Title = new() { Text = "Value" },
            //        Min = 0,
            //        Max = 10
            //    }
            //},
            //Plugins = new()
            //{
            //    Tooltips = new()
            //    {

            //    }
            //},

        };

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Chart.Update();
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            ChartData = new ChartData<object>()
            {
            };
        }

        public enum ViewSpanType
        {
            Day,
            Week,
            Month,
            SixMonth
        }
    }
}