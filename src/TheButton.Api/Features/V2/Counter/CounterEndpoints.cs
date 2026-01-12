using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TheButton.Services;

namespace TheButton.Api.Features.V2.Counter;

public static class CounterEndpoints
{
    public static void MapCounterEndpoints(this IEndpointRouteBuilder app)
    {
        var versionedGroup = app.NewVersionedApi("v2")
            .MapGroup("/api/v{version:apiVersion}/counter")
            .HasApiVersion(new ApiVersion(2, 0));

        versionedGroup.MapPost("", (ICounterService counterService) =>
        {
            var newValue = counterService.Increment();
            return Results.Ok(new CounterResponse(newValue));
        })
        .WithTags("Counter");
    }
}
