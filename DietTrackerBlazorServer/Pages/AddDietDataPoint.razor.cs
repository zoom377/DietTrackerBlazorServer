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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DietTrackerBlazorServer.Pages
{
    public partial class AddDietDataPoint
    {
        List<FoodType> _FoodTypes { get; set; } = new List<FoodType>();
        FoodType _SelectedFoodType { get; set; }

        string _AutoCompleteText { get; set; }

        DateTime _SelectedDate { get; set; } = DateTime.Now;
        bool _UseCurrentDate { get; set; }

        protected override async Task OnInitializedAsync()
        {
            SetAppLoading(true);
            await LoadFoodTypes();
            SetAppLoading(false);
        }

        async Task OnAddButtonClicked()
        {
            using (var dbContext = await _DbContextFactory.CreateDbContextAsync())
            {
                DateTime date = _UseCurrentDate ? DateTime.Now : _SelectedDate;
                FoodType foodType = _SelectedFoodType;

                //If food type doesn't exist, create it
                if (foodType == null)
                {
                    var newFoodType = new FoodType { ApplicationUserId = await GetUserIdAsync(), Name = _AutoCompleteText };
                    await dbContext.AddAsync(newFoodType);
                    await dbContext.SaveChangesAsync();
                    
                }

                FoodDataPoint dataPoint = new FoodDataPoint { ApplicationUserId = await GetUserIdAsync(), Date = date, FoodTypeId = foodType.Id };
                await dbContext.AddAsync(dataPoint);
                await dbContext.SaveChangesAsync();
            }
        }

        async Task LoadFoodTypes()
        {
            using (var dbContext = await _DbContextFactory.CreateDbContextAsync())
            {
                var userId = await GetUserIdAsync();
                var foodTypesQuery = dbContext.FoodTypes.Where(e => e.ApplicationUserId == userId);
                _FoodTypes = await foodTypesQuery.ToListAsync();
            }
        }
    }
}