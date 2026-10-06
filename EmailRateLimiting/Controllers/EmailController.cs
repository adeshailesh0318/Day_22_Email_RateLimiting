using Day22_EmailRateLimiting.Models;
using Day22_EmailRateLimiting.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Day22_EmailRateLimiting.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("EmailPolicy")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail(
            [FromForm] EmailRequest request)
        {
            if (request.Attachment == null)
            {
                return BadRequest("PDF attachment is required.");
            }

            if (!request.Attachment.FileName
                .EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Only PDF files are allowed.");
            }

            await _emailService.SendEmailAsync(
                request.To,
                request.Subject,
                request.Body,
                request.Attachment);

            return Ok(new
            {
                message = "Email sent successfully."
            });
        }
    }
}