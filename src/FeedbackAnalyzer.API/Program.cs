using FeedbackAnalyzer.Application.Interfaces;
using FeedbackAnalyzer.Application.Services;
using FeedbackAnalyzer.Domain.Interfaces;
using FeedbackAnalyzer.Infrastructure.AI;
using FeedbackAnalyzer.Infrastructure.Data;
using FeedbackAnalyzer.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ── Services ──────────────────────────────────────────────────────────────────

// Infrastructure - Repositories (singleton, date pastrate in memorie)
builder.Services.AddSingleton<InMemoryBusinessRepository>();
builder.Services.AddSingleton<IBusinessRepository>(sp =>
    sp.GetRequiredService<InMemoryBusinessRepository>());
builder.Services.AddSingleton<IFeedbackRepository, InMemoryFeedbackRepository>();

// Infrastructure - AI Analyzer (Mock pentru Lab 0; inlocuit cu OpenAI in Phase 3)
builder.Services.AddScoped<ISentimentAnalyzerService, MockSentimentAnalyzer>();

// Application — Services
builder.Services.AddScoped<BusinessService>();
builder.Services.AddScoped<FeedbackService>();

// API
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "AI Customer Feedback Analyzer API",
        Version = "v1",
        Description = "API pentru analiza sentimentului feedback-urilor clientilor cu AI. Lab 0 - MVP."
    });
});

// CORS — permite request-uri din orice origine (dev only)
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// ── Seed Date ─────────────────────────────────────────────────────────────────
var businessRepo = app.Services.GetRequiredService<InMemoryBusinessRepository>();
SeedData.Initialize(businessRepo);

// ── Middleware ────────────────────────────────────────────────────────────────
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Feedback Analyzer API v1");
        options.RoutePrefix = string.Empty; // Swagger la root: https://localhost:PORT/
    });
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

// Health check endpoint
app.MapGet("/api/health", () => Results.Ok(new
{
    status = "Healthy",
    timestamp = DateTime.UtcNow,
    version = "1.0.0-lab0"
}))
.WithName("HealthCheck")
.WithTags("System");

app.Run();
