using DietTrackerBlazorServer.Data;
using DietTrackerBlazorServer.Model;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MimeKit;
using Org.BouncyCastle.Tls;

namespace DietTrackerBlazorServer.Services
{
    public class EmailVerificationService : BackgroundService
    {
        [Inject] public IDbContextFactory<ApplicationDbContext> _DbContextFactory { get; set; }
        [Inject] public UserManager<ApplicationUser> _UserManager { get; set; }
        [Inject] public IConfiguration _cfg { get; set; }

        List<PendingVerificationEmail> _pendingVerificationEmails = new List<PendingVerificationEmail>();

        class PendingVerificationEmail
        {
            public string UserId { get; set; }
            public string Token { get; set; }
            public DateTime ExpiryTime { get; set; }
        }

        public async Task IsVerificationPending(ApplicationUser user)
        {
        }

        public async Task SendVerificationEmail(ApplicationUser user)
        {
            //If there is already one pending, overwrite it
            var current = _pendingVerificationEmails
                .FirstOrDefault(x => x.UserId == user.Id);

            if (current != null)
                _pendingVerificationEmails.Remove(current);

            var duration = _cfg.GetValue<int>("VerificationEmailValidDuration");
            var expiry = DateTime.Now + TimeSpan.FromMinutes(duration);

            PendingVerificationEmail pending = new PendingVerificationEmail
            {
                UserId = user.Id,
                Token = await _UserManager.GenerateEmailConfirmationTokenAsync(user),
                ExpiryTime = expiry
            };

            _pendingVerificationEmails.Add(pending);

            var message = new MimeMessage(_cfg.GetValue<string>("Mail/Username"),
                _cfg.GetValue<string>("Mail/Username"),
                "Health tracker email verification",
                "Please click the link to activate your account.");

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_cfg.GetValue<string>("Mail/Host"), _cfg.GetValue<int>("Mail/Port"), false);
                await client.AuthenticateAsync(_cfg.GetValue<string>("Mail/Username"), _cfg.GetValue<string>("Mail/Password"));
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

        }

        public async Task ConfirmToken(string emailVerificationToken)
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {

                await Task.Delay(10, stoppingToken);
            }
        }
    }
}
