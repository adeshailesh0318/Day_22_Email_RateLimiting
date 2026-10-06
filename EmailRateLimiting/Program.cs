using Day22_EmailRateLimiting.Services;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Email Service
builder.Services.AddScoped<IEmailService, EmailService>();

// Configure Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter(
        policyName: "EmailPolicy",
        options =>
        {
            options.PermitLimit = 3;

            options.Window = TimeSpan.FromMinutes(1);

            options.QueueLimit = 0;

            options.QueueProcessingOrder =
                QueueProcessingOrder.OldestFirst;
        });
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

// Enable Rate Limiting
app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();