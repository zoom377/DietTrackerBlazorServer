using DietTrackerBlazorServer.Data;
using DietTrackerBlazorServer.Model;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MimeKit;
using Org.BouncyCastle.Tls;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System.Text;
using System.Web;

namespace DietTrackerBlazorServer.Services
{
    public class EmailVerificationService
    {
        private readonly ILogger<EmailVerificationService> _Logger;
        private readonly IDbContextFactory<ApplicationDbContext> _DbContextFactory;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IConfiguration _cfg;
        private readonly IWebHostEnvironment _host;

        public EmailVerificationService(ILogger<EmailVerificationService> logger,
            IDbContextFactory<ApplicationDbContext> dbContextFactory,
            UserManager<ApplicationUser> userManager,
            IConfiguration cfg,
            IWebHostEnvironment host)
        {
            _Logger = logger;
            _DbContextFactory = dbContextFactory;
            _UserManager = userManager;
            _cfg = cfg;
            _host = host;
        }

        public async Task SendVerificationEmail(ApplicationUser user)
        {

            string token = await _UserManager.GenerateEmailConfirmationTokenAsync(user);
            string code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            //string code = HttpUtility.UrlEncode(token, Encoding.);
            var confirmationLink = $"https://localhost:7171/Identity/Account/ConfirmEmail?userId={user.Id}&emailToken={code}";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("dt-app", "dtapp@dtapp.test"));
            message.To.Add(new MailboxAddress(_cfg["Mail:Name"], _cfg["Mail:Username"]));
            message.Subject = "Health tracker email verification";
            message.Body = new TextPart("html")
            {
                Text = $"Thanks for registering with Health Tracker!" +
                $"<br/>" +
                $"<a href=\"{confirmationLink}\">Click here to confirm your email address.</a>"
            };

            //Todo:
            //Email sending can be an unreliable operation.
            //Rework this to handle failures (mail server down, credentials incorrect, etc)

            using (var client = new SmtpClient())
            {
                var host = _cfg["Mail:Hostname"];
                var port = _cfg.GetValue<int>("Mail:Port");
                var username = _cfg["Mail:Username"];
                var password = _cfg["Mail:Password"];

                await client.ConnectAsync(host, port, false);
                await client.AuthenticateAsync(username, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

        }

        public async Task<bool> ConfirmEmailAsync(ApplicationUser user, string emailVerificationToken)
        {
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(emailVerificationToken));
            //var decodedToken = HttpUtility.UrlDecode(emailVerificationToken);

            var result = await _UserManager.ConfirmEmailAsync(user, decodedToken);
            return result.Succeeded;
        }
    }
}
