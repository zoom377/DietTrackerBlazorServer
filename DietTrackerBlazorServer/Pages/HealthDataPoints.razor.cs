using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using DietTrackerBlazorServer;
using DietTrackerBlazorServer.Shared;
using DietTrackerBlazorServer.Model;
using Blazorise;
using Blazorise.DataGrid;
using Blazorise.Snackbar;
using Blazorise.Charts;
using DietTrackerBlazorServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DietTrackerBlazorServer.Pages
{
    public partial class HealthDataPoints
    {
        [Inject]
        IDbContextFactory<ApplicationDbContext> DbContextFactory { get; set; }

        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        UserManager<ApplicationUser> UserManager { get; set; }

        List<HealthDataPoint> CurrentHealthDataPoints { get; set; } = new List<HealthDataPoint>();
        HealthDataPoint SelectedHealthDataPoint { get; set; }

        



        protected override async Task OnInitializedAsync()
        {
            using (ApplicationDbContext dbContext = await DbContextFactory.CreateDbContextAsync())
            {
                
            }
        }
    }
}