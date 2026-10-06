using Microsoft.AspNetCore.Http;

namespace Day22_EmailRateLimiting.Models
{
    public class EmailRequest
    {
        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public IFormFile Attachment { get; set; }
    }
}