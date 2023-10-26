using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;
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
using MudBlazor;
using DietTrackerBlazorServer.Components;
using NuGet.Protocol;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DietTrackerBlazorServer.Pages
{
    public partial class FoodTypes : DTComponentBase
    {
        [Inject] ISnackbar _Snackbar { get; set; }
        protected List<FoodType> _FoodTypes { get; set; }
        protected FoodType _SelectedItem { get; set; }
        protected MudDataGrid<FoodType> _Grid { get; set; }
        protected DTDialog<FoodType> _Dialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            using var dbc = await _DbContextFactory.CreateDbContextAsync();
            var userId = await GetUserIdAsync();

            _FoodTypes = dbc.FoodTypes
                .Where(x => x.ApplicationUserId == userId)
                .ToList();

        }

        protected async Task OnAddItem()
        {
            FoodType newFoodType = new FoodType();
            var userId = await GetUserIdAsync();
            newFoodType.ApplicationUserId = userId;
            var confirm = await _Dialog.ShowAsync(newFoodType);
            if (confirm)
            {
                using var dbc = await _DbContextFactory.CreateDbContextAsync();
                dbc.Add(newFoodType);
                var cc = await dbc.SaveChangesAsync();
                if (cc > 0)
                {
                    _FoodTypes.Add(newFoodType);
                }
            }
        }

        protected async Task OnEditItem(FoodType item)
        {
            using var dbc = await _DbContextFactory.CreateDbContextAsync();
            var confirm = await _Dialog.ShowAsync(item, DialogMode.Edit);

            if (confirm)
            {
                try
                {
                    dbc.Update(item);
                    var cc = await dbc.SaveChangesAsync();
                }
                catch
                {
                    _Snackbar.Add($"Error", Severity.Error);
                    throw;
                }
                finally
                {
                    _Snackbar.Add($"{item.Name} edited succesfully", Severity.Success);
                }
            }
            else
            {
                await dbc.Entry(item).ReloadAsync();
            }
        }

        //protected async Task OnEditItem(FoodType item)
        //{
        //    using var dbc = await _DbContextFactory.CreateDbContextAsync();
        //    var confirm = await _Dialog.ShowAsync(item, DialogMode.Edit);
        //    if (confirm)
        //    {
        //        dbc.Update(item);
        //        var cc = await dbc.SaveChangesAsync();
        //        if (cc > 0)
        //        {
        //            _Snackbar.Add($"{item.Name} edited succesfully", Severity.Success);
        //        }
        //        else
        //        {
        //            await dbc.Entry(item).ReloadAsync();
        //        }
        //    }
        //    else
        //    {
        //        await dbc.Entry(item).ReloadAsync();
        //    }
        //}

        protected async Task OnDeleteItem(FoodType item)
        {
            using var dbc = await _DbContextFactory.CreateDbContextAsync();
            var userId = await GetUserIdAsync();

            var foodDataPoints = dbc.FoodDataPoints
                .Where(x => x.ApplicationUserId == userId && x.FoodTypeId == item.Id);

            try
            {

                dbc.RemoveRange(foodDataPoints);
                dbc.Remove(item);
                var cc = await dbc.SaveChangesAsync();
            }
            catch
            {
                _Snackbar.Add("Failure", Severity.Error);
            }
            finally
            {
                _Snackbar.Add($"Removed {item.Name} and all associated data points.");
                _FoodTypes.Remove(item);
            }
        }

    }
}