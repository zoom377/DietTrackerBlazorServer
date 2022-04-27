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

namespace DietTrackerBlazorServer.Pages
{
    public partial class EditHealthMetrics
    {
        protected bool Loaded { get; set; }
        protected IEnumerable<HealthMetric> HealthMetrics { get; set; }
        public HealthMetric DataGridSelectedHealthMetric { get; set; }

        protected Modal AddModal { get; set; }

        protected HealthMetric NewHealthMetric { get; set; } = new HealthMetric();

        public void OnModalAddButtonClicked()
        {
        }

        public void OnAddNewMetricButtonClicked()
        {
        }

        protected override Task OnInitializedAsync()
        {
            HealthMetrics = new List<HealthMetric> { 
            new HealthMetric{Name = "Anxiety", Description = "How anxious" },
            new HealthMetric{Name = "Energy", Description = "How energetic" },
            new HealthMetric{Name = "Skin", Description = "Skin quality" }
            };

            Loaded = true;
            return Task.CompletedTask;
        }
    }
}