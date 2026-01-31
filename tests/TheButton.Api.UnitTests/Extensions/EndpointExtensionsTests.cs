using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using TheButton.Api.Extensions;

namespace TheButton.Api.UnitTests.Extensions;

[TestClass]
public class EndpointExtensionsTests
{
    [TestMethod]
    public void MapEndpoints_RegistersExpectedRoutes()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        app.MapEndpoints();

        var patterns = ((IEndpointRouteBuilder)app).DataSources
            .SelectMany(source => source.Endpoints)
            .OfType<RouteEndpoint>()
            .Select(endpoint => endpoint.RoutePattern.RawText)
            .Where(pattern => !string.IsNullOrWhiteSpace(pattern))
            .ToList();

        Assert.IsTrue(patterns.Any(pattern => pattern is "/api/v2/counter" or "/api/v2/counter/"));
        Assert.IsTrue(patterns.Any(pattern => pattern is "/api/v3/counter" or "/api/v3/counter/"));
        Assert.IsTrue(patterns.Any(pattern => pattern == "/api/v3/counter/{userId:guid}"));
        Assert.IsTrue(patterns.Any(pattern => pattern is "/health" or "/health/"));
    }
}
