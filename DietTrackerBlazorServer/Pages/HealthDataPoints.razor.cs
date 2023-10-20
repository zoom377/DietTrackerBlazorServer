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
    public partial class HealthDataPoints : DTComponentBase
    {
        List<HealthDataPoint> _CurrentHealthDataPoints { get; set; } = new List<HealthDataPoint>();
        HealthDataPoint _SelectedHealthDataPoint { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            SetAppLoading(true);
            using (ApplicationDbContext dbContext = await _DbContextFactory.CreateDbContextAsync())
            {
                var userId = await GetUserIdAsync();
                var query = dbContext.HealthDataPoints
                    .Where(e => e.ApplicationUserId == userId)
                    .Include(e => e.HealthMetric)
                    .OrderByDescending(e => e.Date)
                    .AsNoTracking();

                _CurrentHealthDataPoints = await query.ToListAsync();
            }
            SetAppLoading(false);
        }
    }
}