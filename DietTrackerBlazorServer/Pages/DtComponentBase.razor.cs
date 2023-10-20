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
    public partial class DTComponentBase : ComponentBase
    {
        [Inject]
        protected UserManager<ApplicationUser> _UserManager { get; set; }
        [Inject]
        protected AuthenticationStateProvider _AuthenticationStateProvider { get; set; }
        [Inject]
        protected IDbContextFactory<ApplicationDbContext> _DbContextFactory { get; set; }
        [CascadingParameter]
        protected SnackbarStack _SnackbarStack { get; set; }
        [CascadingParameter]
        protected Action<bool> SetAppLoading { get; set; }

        protected async Task<ApplicationUser> GetUser()
        {
            var authState = await _AuthenticationStateProvider.GetAuthenticationStateAsync();
            return await _UserManager.GetUserAsync(authState.User);
        }

        protected async Task<string> GetUserIdAsync()
        {
            return await _UserManager.GetUserIdAsync(await GetUser());
        }

        


    }
}