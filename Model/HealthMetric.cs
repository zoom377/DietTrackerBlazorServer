using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DietTrackerBlazorWASM.Shared.Model
{

    public class HealthMetric
    {
        public int Id { get; set; }
        public int IdentityUserId { get; set; }
        public IdentityUser User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
