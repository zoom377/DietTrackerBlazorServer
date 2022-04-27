using DietTrackerBlazorServer.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DietTrackerBlazorServer.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<HealthMetric> HealthMetric { get; set; }
        public DbSet<HealthDataPoint> HealthDataPoints { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}