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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DietTrackerBlazorServer.Pages
{
    public partial class DTComponentBase : ComponentBase
    {
        [Inject]
        protected AuthenticationStateProvider _AuthenticationStateProvider { get; set; }
        [Inject]
        protected IDbContextFactory<ApplicationDbContext> _DbContextFactory { get; set; }

        protected async Task<ClaimsPrincipal> GetUser()
        {
            var authState = await _AuthenticationStateProvider.GetAuthenticationStateAsync();
            return authState.User;
        }

        protected async Task<string> GetUserIdAsync()
        {
            var user = await GetUser();
            var userId = user.FindFirst(u => u.Type.Contains("nameidentifier"))?.Value;
            return userId;
        }

        


    }
}