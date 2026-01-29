using Scalar.AspNetCore;
using TheButton.Api.Extensions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TheButton.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

// V2 services (backward compatibility)
builder.Services.AddSingleton<TheButton.Application.Counter.V2.Increment.ICounterService, TheButton.Infrastructure.Counter.V2.CounterService>();
builder.Services.AddScoped<TheButton.Application.Counter.V2.Increment.IncrementHandler>();

// V3 services
builder.Services.AddScoped<TheButton.Application.Abstractions.ICounterWriter, TheButton.Infrastructure.Counter.SqlCounterWriter>();
builder.Services.AddScoped<TheButton.Application.Abstractions.ICounterReadRepository, TheButton.Infrastructure.Counter.SqlCounterReadRepository>();
builder.Services.AddScoped<TheButton.Application.Counter.V3.Increment.IncrementHandler>();
builder.Services.AddScoped<TheButton.Application.Counter.V3.GetGlobal.GetGlobalQueryHandler>();
builder.Services.AddScoped<TheButton.Application.Counter.V3.GetUser.GetUserCountersQueryHandler>();

builder.Services.AddDbContext<TheButtonDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Sql"), 
    sqlOptions =>sqlOptions.EnableRetryOnFailure(
        maxRetryCount: 10,
        maxRetryDelay: TimeSpan.FromSeconds(30),
        errorNumbersToAdd: null)));

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        var origins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

        policy.WithOrigins(origins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    // Dev-only auto-migrate (guarded)
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<TheButtonDbContext>();
    db.Database.Migrate();
}

app.UseCors("AllowFrontend");

app.MapEndpoints();

app.Run();

public partial class Program { }
