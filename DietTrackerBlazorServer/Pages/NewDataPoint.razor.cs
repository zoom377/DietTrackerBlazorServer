using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using DietTrackerBlazorServer.Model;

namespace DietTrackerBlazorServer.Pages
{
    public partial class NewDataPoint
    {
        public HealthDataPoint HealthDataPoint { get; set; } = new HealthDataPoint {Date = DateTime.Now };
        public bool Loaded { get; set; } = false;

        async void Submit()
        {
            //logger.LogInformation($"Submitted data:\nDate:{HealthDataPoint.Date.ToLongTimeString()}\nAnxiety:{HealthDataPoint.Anxiety}");
            //var response = await Client.PostAsync("api/HealthDataPoint", new JsonContent( JsonSerializer.Serialize(HealthDataPoint), ));
            //var response = await Client.PostAsJsonAsync("api/healthdatapoint", HealthDataPoint);
            
        }

        protected override Task OnInitializedAsync()
        {
            var xx = base.OnInitializedAsync();
            Loaded = true;
            return xx;
        }
    }
}