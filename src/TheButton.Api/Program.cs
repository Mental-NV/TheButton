using Scalar.AspNetCore;
using TheButton.Services;
using Asp.Versioning;
using TheButton.Api.Extensions;
using System.Reflection;
using TheButton.Api.Features.V2.Counter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpoints(typeof(Program).Assembly);
builder.Services.AddSingleton<ICounterService, CounterService>();
builder.Services.AddOpenApi();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(3, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

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
}

app.UseCors("AllowFrontend");

var versionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(3, 0))
    .ReportApiVersions()
    .Build();

app.NewVersionedApi("v3")
   .MapGroup("/api/v3")
   .HasApiVersion(new ApiVersion(3, 0))
   .MapEndpoints();

app.MapCounterEndpoints();

app.Run();

public partial class Program { }
