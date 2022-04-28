using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DietTrackerBlazorServer.Model
{

    public class HealthMetric
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

        public HealthMetric ShallowCopy()
        {
            return (HealthMetric)MemberwiseClone();
        }
    }
}
