using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.AspNetCore.Http;

namespace Day22_EmailRateLimiting.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(
            string to,
            string subject,
            string body,
            IFormFile attachment)
        {
            var emailSettings = _configuration
                .GetSection("EmailSettings");

            var host = emailSettings["Host"];
            var port = int.Parse(emailSettings["Port"]);
            var username = emailSettings["UserName"];
            var password = emailSettings["Password"];
            var fromEmail = emailSettings["FromEmail"];

            var email = new MimeMessage();

            email.From.Add(
                new MailboxAddress("Day 22 Application", fromEmail));

            email.To.Add(
                MailboxAddress.Parse(to));

            email.Subject = subject;

            var builder = new BodyBuilder();

            builder.TextBody = body;

            if (attachment != null && attachment.Length > 0)
            {
                using var memoryStream = new MemoryStream();

                await attachment.CopyToAsync(memoryStream);

                builder.Attachments.Add(
                    attachment.FileName,
                    memoryStream.ToArray());
            }

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                host,
                port,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                username,
                password);

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}