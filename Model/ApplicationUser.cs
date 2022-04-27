using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietTrackerBlazorServer.Model
{
    public class ApplicationUser : IdentityUser
    {
        public List<HealthMetric> HealthMetrics { get; set; }
        public List<HealthDataPoint> HealthDataPoints { get; set; }

    }
}
