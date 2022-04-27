using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DietTrackerBlazorServer.Model
{

    public class HealthMetric
    {
        public int HealthMetricId { get; set; }
        public string IdentityUserId { get; set; }
        public IdentityUser IndentityUser { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
