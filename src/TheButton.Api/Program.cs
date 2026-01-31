using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;
using TheButton.Api.Extensions;
using TheButton.Infrastructure.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// V2 services (backward compatibility)
_ = builder.Services.AddSingleton<TheButton.Application.Counter.V2.Increment.ICounterService, TheButton.Infrastructure.Counter.V2.CounterService>();
_ = builder.Services.AddScoped<TheButton.Application.Counter.V2.Increment.IncrementHandler>();

// V3 services
_ = builder.Services.AddScoped<TheButton.Application.Abstractions.ICounterWriter, TheButton.Infrastructure.Counter.SqlCounterWriter>();
_ = builder.Services.AddScoped<TheButton.Application.Abstractions.ICounterReadRepository, TheButton.Infrastructure.Counter.SqlCounterReadRepository>();
_ = builder.Services.AddScoped<TheButton.Application.Counter.V3.Increment.IncrementHandler>();
_ = builder.Services.AddScoped<TheButton.Application.Counter.V3.GetGlobal.GetGlobalQueryHandler>();
_ = builder.Services.AddScoped<TheButton.Application.Counter.V3.GetUser.GetUserCountersQueryHandler>();

_ = builder.Services.AddDbContext<TheButtonDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("Sql"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)));

_ = builder.Services.AddOpenApi();

_ = builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        string[] origins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

        _ = policy.WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.MapOpenApi();
    _ = app.MapScalarApiReference();

    // Dev-only auto-migrate (guarded)
    await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
    TheButtonDbContext db = scope.ServiceProvider.GetRequiredService<TheButtonDbContext>();
    await db.Database.MigrateAsync().ConfigureAwait(false);
}

_ = app.UseCors("AllowFrontend");

app.MapEndpoints();

await app.RunAsync().ConfigureAwait(false);

/// <summary>
/// Marker type for WebApplicationFactory access in tests.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed partial class Program
{
    private Program()
    {
    }
}
