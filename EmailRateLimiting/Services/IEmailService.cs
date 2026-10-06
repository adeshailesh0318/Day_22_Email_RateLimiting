using Microsoft.AspNetCore.Http;

namespace Day22_EmailRateLimiting.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(
            string to,
            string subject,
            string body,
            IFormFile attachment);
    }
}