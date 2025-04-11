//using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Security;

namespace HarshitCommunications.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:FromEmail"]));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

            using var emailClient = new SmtpClient();
            emailClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            emailClient.Authenticate(
                _configuration["EmailSettings:FromEmail"],
                _configuration["EmailSettings:AppPassword"]
            );
            emailClient.Send(emailToSend);
            emailClient.Disconnect(true);

            return Task.CompletedTask;
        }
    }
}