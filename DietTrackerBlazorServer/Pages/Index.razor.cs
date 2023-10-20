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
using DietTrackerBlazorServer.Pages;
using DietTrackerBlazorServer.Shared;
using DietTrackerBlazorServer.Data;
using DietTrackerBlazorServer.Model;
using Blazorise;
using Blazorise.DataGrid;
using Blazorise.Snackbar;
using Blazorise.Charts;
using Blazorise.Components;
using Microsoft.AspNetCore.Razor.Language.Extensions;

namespace DietTrackerBlazorServer.Pages
{
    public partial class Index : DTComponentBase
    {
        public async Task Clicked()
        {
            using var dbContext = await _DbContextFactory.CreateDbContextAsync();

            //Clear all existing data
            dbContext.RemoveRange(dbContext.FoodDataPoints);
            dbContext.RemoveRange(dbContext.HealthDataPoints);
            dbContext.RemoveRange(dbContext.FoodTypes);
            dbContext.RemoveRange(dbContext.HealthMetrics);

            await dbContext.SaveChangesAsync();

            dbContext.AddRange(MockDataGenerator.GenerateData(await GetUserIdAsync(), TimeSpan.FromDays(60)));
            await dbContext.SaveChangesAsync();

        }
    }
}