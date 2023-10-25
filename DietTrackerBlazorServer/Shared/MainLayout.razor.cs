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
using DietTrackerBlazorServer.Data;
using DietTrackerBlazorServer.Model;
using Microsoft.AspNetCore.Identity;
using DietTrackerBlazorServer.Pages;
using MudBlazor;

namespace DietTrackerBlazorServer.Shared
{
    public partial class MainLayout : DTComponentBase
    {

        //LayoutView automatically passes a renderfragment named body to the layout that is specified.
        [Parameter]
        public RenderFragment? Body { get; set; }

        public bool _drawerOpen { get; set; } = true;

        protected MudTheme Theme { get; set; } = new MudTheme();


        //protected bool _Loading { get; set; }
        //protected string _UserName { get; set; }


        protected override async Task OnInitializedAsync()
        {
            //Loading = true;
            //var authState = await _AuthProvider.GetAuthenticationStateAsync();
            //var user = await _UserManager.GetUserAsync(authState.User);
            //if (user == null)
            //{
            //    _UserName = "None";
            //}
            //else
            //{
            //    _UserName = await _UserManager.GetUserNameAsync(user);
            //}

        }

        protected void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        protected void SetAppLoading(bool appLoading)
        {
            StateHasChanged();
        }
    }
}