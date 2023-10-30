using DietTrackerBlazorServer.Data;
using DietTrackerBlazorServer.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DietTrackerBlazorServer.Services
{
    public class EmailVerificationManager
    {
        [Inject] public IDbContextFactory<ApplicationDbContext> _DbContextFactory { get; set; }
        [Inject] public UserManager<ApplicationUser> _UserManager { get; set; }

        public async Task IsVerificationPending(ApplicationUser user)
        {

        }

        public async Task SendVerificationEmail(ApplicationUser user)
        {

        }
    }
}
