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
        List<HealthMetric> _HealthMetrics { get; set; } = new List<HealthMetric>();
        List<HealthDataPoint> _DataPoints { get; set; } = new List<HealthDataPoint>();
        bool _UseCurrentDate { get; set; } = true;
        DateTime _SelectedDate { get; set; } = DateTime.Now;
        bool _Loaded { get; set; } = false;


        protected override async Task OnInitializedAsync()
        {
            using var dbc = await _DbContextFactory.CreateDbContextAsync();
            var userId = await GetUserIdAsync();

            _HealthMetrics = dbc.HealthMetrics
                .Where(x => x.ApplicationUserId == userId)
                .ToList();

            _DataPoints.Clear();
            foreach (var metric in _HealthMetrics)
            {
                _DataPoints.Add(new HealthDataPoint()
                {
                    ApplicationUserId = userId,
                    HealthMetricId = metric.Id,
                    Value = 5
                });
            }
        }

        async Task OnSubmitButtonClicked()
        {
            
            
            
        }
    }
}