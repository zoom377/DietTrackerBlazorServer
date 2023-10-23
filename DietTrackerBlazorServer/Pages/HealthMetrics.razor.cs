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
using DietTrackerBlazorServer.Model;
using DietTrackerBlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DietTrackerBlazorServer.Shared;

namespace DietTrackerBlazorServer.Pages
{
    public partial class HealthMetrics : DTComponentBase
    {

        List<HealthMetric> _HealthMetrics { get; set; }


        protected override async Task OnInitializedAsync()
        {
            await ReloadData();
        }

        protected async Task ReloadData()
        {
            using (ApplicationDbContext dbContext = await _DbContextFactory.CreateDbContextAsync())
            {
                var userId = await GetUserIdAsync();

                _HealthMetrics = await dbContext.HealthMetrics
                    .Where(e => e.ApplicationUserId == userId)
                    .ToListAsync();
            }
        }

        //protected async Task OnAddButtonClicked()
        //{
        //    _SubjectHealthMetric = new HealthMetric();
        //}

        //protected async Task OnEditButtonClicked()
        //{
        //    _SubjectHealthMetric = _DataGridSelectedHealthMetric.ShallowCopy();
        //}

        //protected async Task OnDeleteButtonClicked()
        //{
        //    _SubjectHealthMetric = _DataGridSelectedHealthMetric;
        //}

        //protected async Task OnValidSubmit_AddHealthMetric()
        //{
        //    _SubjectHealthMetric.ApplicationUserId = await GetUserIdAsync();

        //    using (ApplicationDbContext dbContext = await _DbContextFactory.CreateDbContextAsync())
        //    {
        //        var query = dbContext.HealthMetrics
        //                .Where(m => m.ApplicationUserId == _SubjectHealthMetric.ApplicationUserId)
        //                .Where(m => m.Name == _SubjectHealthMetric.Name);

        //        if (await query.AnyAsync())
        //        {
        //        }
        //        else
        //        {
        //            await dbContext.AddAsync(_SubjectHealthMetric);
        //            if (await dbContext.SaveChangesAsync() > 0)
        //            {
        //            }
        //            else
        //            {
        //            }
        //        }

        //    }
        //    await ReloadData();
        //}

        //protected async Task OnValidSubmit_EditHealthMetric()
        //{
        //    using (ApplicationDbContext dbContext = await _DbContextFactory.CreateDbContextAsync())
        //    {
        //        var query = dbContext.HealthMetrics
        //                .Where(m => m.ApplicationUserId == _SubjectHealthMetric.ApplicationUserId)
        //                .Where(m => m.Id != _SubjectHealthMetric.Id)
        //                .Where(m => m.Name == _SubjectHealthMetric.Name);
                

        //        if (await query.AnyAsync())
        //        {
        //        }
        //        else
        //        {
        //            dbContext.Update(_SubjectHealthMetric);
        //            if (await dbContext.SaveChangesAsync() > 0)
        //            {
        //            }
        //            else
        //            {
        //            }

        //        }
        //    }
        //    await ReloadData();
        //}

        //protected async Task OnDeleteHealthMetricConfirmed()
        //{
        //    using (ApplicationDbContext dbContext = await _DbContextFactory.CreateDbContextAsync())
        //    {
        //        var userId = await GetUserIdAsync();

        //        var dependentDataPoints = dbContext.HealthDataPoints
        //            .Where(e => e.ApplicationUserId == userId)
        //            .Where(e => e.HealthMetricId == _SubjectHealthMetric.Id);

        //        dbContext.RemoveRange(dependentDataPoints);
        //        //foreach (var dataPoint in associatedDataPoints)
        //        //{
        //        //    dbContext.Remove(dataPoint);
        //        //}

        //        dbContext.Remove(_SubjectHealthMetric);
        //        if (await dbContext.SaveChangesAsync() > 0)
        //        {
        //        }
        //        else
        //        {
        //        }
        //    }
        //    _DataGridSelectedHealthMetric = null;
        //    await ReloadData();
        //}
    }
}