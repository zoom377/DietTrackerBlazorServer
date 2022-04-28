using DietTrackerBlazorServer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DietTrackerBlazorServer.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{

    public DbSet<HealthMetric> HealthMetrics { get; set; }
    public DbSet<HealthDataPoint> HealthDataPoints { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
