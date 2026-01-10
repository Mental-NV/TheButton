var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

int counter = 0;

app.MapPost("/api/v2/counter", () =>
{
    var newValue = Interlocked.Increment(ref counter);
    return Results.Ok(new { value = newValue });
});

app.Run();
