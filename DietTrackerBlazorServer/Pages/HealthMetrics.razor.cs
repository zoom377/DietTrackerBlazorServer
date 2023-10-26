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
using MudBlazor;
using DietTrackerBlazorServer.Components;
using MudBlazor.Utilities;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using System.Runtime.ExceptionServices;

namespace DietTrackerBlazorServer.Pages
{
    public partial class HealthMetrics : DTComponentBase
    {
        [Inject] ISnackbar _Snackbar { get; set; }
        [CascadingParameter] MudTheme _Theme { get; set; }

        MudDataGrid<HealthMetric> _Grid { get; set; }
        List<HealthMetric> _HealthMetrics { get; set; }
        HealthMetric _SelectedItem { get; set; }
        DTDialog<HealthMetric> _Dialog { get; set; }

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

        async Task SelectedItemChanged(HealthMetric metric)
        {
            _SelectedItem = metric;
        }

        async Task OnItemAdd()
        {
            HealthMetric newMetric = new HealthMetric();
            bool confirmed = await _Dialog.ShowAsync(newMetric, DialogMode.Add); //Wait for user to close dialog
            if (confirmed)
            {
                if (await UserIsAuthenticated())
                {
                    using var dbc = await _DbContextFactory.CreateDbContextAsync();
                    newMetric.ApplicationUserId = await GetUserIdAsync();
                    _HealthMetrics.Add(newMetric);
                    dbc.HealthMetrics.UpdateRange(_HealthMetrics);
                    var changeCount = await dbc.SaveChangesAsync();
                    if (changeCount != 0)
                    {
                        _Snackbar.Add("Added health metric", Severity.Success);
                    }
                    else
                    {
                        _Snackbar.Add("Failed to add health metric", Severity.Error);
                    }
                }
            }
        }
        async Task OnItemEdit(HealthMetric metric)
        {
            HealthMetric editedMetric = new HealthMetric();
            editedMetric.Name = metric.Name;
            editedMetric.Description = metric.Description;
            editedMetric.Color = metric.Color;
            bool confirmed = await _Dialog.ShowAsync(editedMetric, DialogMode.Edit); //Wait for user to close dialog
            if (confirmed)
            {
                metric.Name = editedMetric.Name;
                metric.Description = editedMetric.Description;
                metric.Color = editedMetric.Color;
            }
        }
        async Task OnItemDelete(HealthMetric metric)
        {
            bool confirmed = await _Dialog.ShowAsync(metric, DialogMode.Delete); //Wait for user to close dialog
            if (confirmed)
            {
                using var dbc = await _DbContextFactory.CreateDbContextAsync();
                var userId = await GetUserIdAsync();

                //Delete all data points that rely on this metric before removing it.
                var dataPoints = dbc.HealthDataPoints
                    .Where(x => x.ApplicationUserId == userId && x.HealthMetricId == metric.Id);

                dbc.RemoveRange(dataPoints);
                dbc.Remove(metric);

                var changeCount = await dbc.SaveChangesAsync();
                if (changeCount > 0)
                {
                    _Snackbar.Add($"Removed {metric.Name}", Severity.Success);
                    _HealthMetrics.Remove(metric);
                }
                else
                {
                    _Snackbar.Add($"Failed to remove {metric.Name}", Severity.Error);
                }
            }
        }


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