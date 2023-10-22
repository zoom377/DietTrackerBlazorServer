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

namespace DietTrackerBlazorServer.Pages
{
    public partial class MetricStats
    {


        private HealthMetric SelectedHealthMetric { get; set; }
        private List<HealthMetric> HealthMetrics { get; set; }

        protected override async Task OnInitializedAsync()
        {
            using var dbc = await _DbContextFactory.CreateDbContextAsync();

            var userId = await GetUserIdAsync();

            HealthMetrics = dbc.HealthMetrics.Where(m => m.ApplicationUserId == userId)
                .ToList();
        }


        async Task OnSelectedHealthMetricChanged(HealthMetric newValue)
        {
            SelectedHealthMetric = newValue;
        }

    }
}