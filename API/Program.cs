using Microsoft.EntityFrameworkCore;
using NotificationAPI.Infrastructure.Data;
using NotificationAPI.Infrastructure.Repositories;
using NotificationAPI.Application.Interfaces;
using NotificationAPI.Application.UseCases;
using NotificationAPI.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

// ── SERVICES ────────────────────────────────────────────────────────────────

builder.Services.AddControllers();

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// ── DATABASE ─────────────────────────────────────────────────────────────
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=notifications.db";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// ── REPOSITORY PATTERN ─────────────────────────────────────────

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// ── USE CASES ──────────────────────────────────────────────────

// The use cases are registered directly by their concrete type
// because the controllers inject them by type, not by interface.
builder.Services.AddScoped<SendNotificationUseCase>();
builder.Services.AddScoped<GetNotificationsUseCase>();

// ── VALIDATORS (FluentValidation) ────────────────────────────────────────────

// The validator is registered so that the use cases can inject it if they need it.
builder.Services.AddScoped<SendNotificationRequestValidator>();

// ── BUILD ─────────────────────────────────────────────────────────────────────

var app = builder.Build();

// ── MIDDLEWARE  ──────────────────────────────────────

if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NotificationAPI v1"));
}

// Redirect HTTP to HTTPS automatically
app.UseHttpsRedirection();

// UseAuthorization() enables the authorization system (necessary even if we don't have [Authorize] yet)
app.UseAuthorization();

// MapControllers() connects the controller routes to the HTTP pipeline
app.MapControllers();

// ── DATABASE INITIALIZATION ──────────────────────────────────────────

// EnsureCreated() creates the tables in SQLite if they don't exist.
// In production, db.Database.Migrate() would be used to apply migrations in order.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

// Run() starts the server and begins listening for HTTP requests
app.Run();

// This line makes Program visible to integration tests with WebApplicationFactory
public partial class Program { }
