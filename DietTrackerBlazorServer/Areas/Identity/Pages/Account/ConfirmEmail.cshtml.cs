using DietTrackerBlazorServer.Model;
using DietTrackerBlazorServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DietTrackerBlazorServer.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly EmailVerificationService _emailVerification;
        private readonly UserManager<ApplicationUser> _userManager;

        //[BindProperty(SupportsGet = true)]
        //string EmailToken { get; set; } = string.Empty;

        //[BindProperty(SupportsGet = true)]
        //string UserId { get; set; } = string.Empty;

        public bool success { get; set; }

        public ConfirmEmailModel(EmailVerificationService emailVerification, UserManager<ApplicationUser> userManager)
        {
            this._emailVerification = emailVerification;
            _userManager = userManager;
        }

        public async Task<ActionResult> OnGet(string userId, string emailToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            success = await _emailVerification.ConfirmEmailAsync(user, emailToken);

            return Page();
        }
    }
}
