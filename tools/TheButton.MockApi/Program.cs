WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
_ = builder.Services.AddOpenApi();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.MapOpenApi();
}

int counter = 0;

_ = app.MapPost("/api/v3/counter", () =>
{
    int newValue = Interlocked.Increment(ref counter);
    return Results.Ok(new { value = newValue });
});

await app.RunAsync().ConfigureAwait(false);
