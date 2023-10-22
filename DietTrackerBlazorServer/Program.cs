using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using DietTrackerBlazorServer.Model;
using DietTrackerBlazorServer.Data;
using DietTrackerBlazorServer.Areas.Identity;
using DietTrackerBlazorServer.Services;
using DietTrackerBlazorServer;
using MathNet.Numerics;
using Microsoft.AspNetCore.DataProtection;
using MudBlazor.Services;

internal class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        AddDbContext(builder);

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        //Todo encryption
        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo("/var/www/keys/"));

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        builder.Services.AddMudServices();
        //builder.Services.AddBlazorise(options =>
        //{
        //    options.Immediate = true;
        //})
        //    .AddBootstrapProviders()
        //    .AddFontAwesomeIcons();

        builder.Services.AddTransient<ICorrelationCalculator, PearsonsRCorrelation>();


        var app = builder.Build();

        //init database
        InitialiseDatabase(app);


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor
            | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
        });

        //app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        //app.UseWebSockets();

        app.MapControllers();
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Logger.LogInformation($"\"{app.Environment.ApplicationName}\" environment is \"{app.Environment.EnvironmentName}\"");

        app.Run();

    }
    static void AddDbContext(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        var sqlVersion = new MariaDbServerVersion(new Version(1, 0));
        var dbContextBuilder = (DbContextOptionsBuilder options) =>
        {
            options.UseMySql(connectionString, sqlVersion);
            options.LogTo(Console.WriteLine, LogLevel.Information);
            if (builder.Environment.IsDevelopment())
            {
                //options.EnableSensitiveDataLogging();
                //options.EnableDetailedErrors();
            }
        };

        builder.Services.AddDbContextFactory<ApplicationDbContext>(dbContextBuilder);
    }

    static void InitialiseDatabase(WebApplication app)
    {
        var dbcFactory = app.Services.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
        using var scope = dbcFactory.CreateDbContext();

        scope.Database.Migrate();

    //    using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
    //    {
    //        scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
    //        scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
    //        ...
    //}
    }

}