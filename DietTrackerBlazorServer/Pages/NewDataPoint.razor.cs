using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using DietTrackerBlazorWASM.Client;
using DietTrackerBlazorWASM.Client.Shared;
using Blazorise;
using DietTrackerBlazorWASM.Shared.Model;
using System.Text.Json;

namespace DietTrackerBlazorWASM.Client.Pages
{
    public partial class NewDataPoint
    {
        [Inject]
        public HttpClient Client { get; set; }

        public HealthDataPoint HealthDataPoint { get; set; } = new HealthDataPoint {Date = DateTime.Now };
        public bool Loaded { get; set; } = false;

        async void Submit()
        {
            //logger.LogInformation($"Submitted data:\nDate:{HealthDataPoint.Date.ToLongTimeString()}\nAnxiety:{HealthDataPoint.Anxiety}");
            //var response = await Client.PostAsync("api/HealthDataPoint", new JsonContent( JsonSerializer.Serialize(HealthDataPoint), ));
            var response = await Client.PostAsJsonAsync("api/healthdatapoint", HealthDataPoint);
            
        }

        protected override Task OnInitializedAsync()
        {
            var xx = base.OnInitializedAsync();
            Loaded = true;
            return xx;
        }
    }
}