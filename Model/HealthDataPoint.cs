using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietTrackerBlazorServer.Model
{
    public class HealthDataPoint
    {
        public int HealthDataPointId { get; set; }
        public int HealthMetricId { get; set; }
        public HealthMetric HealthMetric { get; set; }
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
        public DateTime Date { get; set; }
        public int Value { get; set; }

    }
}
