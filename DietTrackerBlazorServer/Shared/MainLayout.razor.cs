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
        protected UserManager<ApplicationUser> UserManager { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthProvider { get; set; }

        bool Loading { get; set; } = true;
        protected string UserEmail { get; set; }

        protected Bar Bar { get; set; }

        protected SnackbarStack SnackbarStack { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthProvider.GetAuthenticationStateAsync();
            var user = await UserManager.GetUserAsync(authState.User);
            if (user == null)
            {
                UserEmail = "None";
            }
            else
            {
                UserEmail = await UserManager.GetEmailAsync(user);
            }

            Loading = false;
        }
    }
}