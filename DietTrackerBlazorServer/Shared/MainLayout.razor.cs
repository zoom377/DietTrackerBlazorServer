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
using Blazorise;
using Blazorise.DataGrid;
using Blazorise.Snackbar;
using Blazorise.Charts;
using Microsoft.AspNetCore.Identity;

namespace DietTrackerBlazorServer.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        protected UserManager<ApplicationUser> _UserManager { get; set; }

        [Inject]
        protected AuthenticationStateProvider _AuthProvider { get; set; }


        protected bool _Loading { get; set; }
        protected string _UserName { get; set; }

        protected Bar _SideBar { get; set; }

        protected SnackbarStack _SnackbarStack { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //Loading = true;
            var authState = await _AuthProvider.GetAuthenticationStateAsync();
            var user = await _UserManager.GetUserAsync(authState.User);
            if (user == null)
            {
                _UserName = "None";
            }
            else
            {
                _UserName = await _UserManager.GetUserNameAsync(user);
            }

        }


        protected void SetAppLoading(bool appLoading)
        {
            _Loading = appLoading;
            StateHasChanged();
        }
    }
}