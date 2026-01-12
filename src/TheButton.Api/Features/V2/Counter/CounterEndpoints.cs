using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TheButton.Api.Abstractions;

namespace TheButton.Api.Features.V2.Counter;

public class CounterEndpoints : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        var versionedGroup = app.NewVersionedApi("v2")
            .MapGroup("/api/v{version:apiVersion}/counter")
            .HasApiVersion(new ApiVersion(2, 0));

        versionedGroup.MapPost("", Increment)
            .WithTags("Counter");
    }

    public static Microsoft.AspNetCore.Http.HttpResults.Ok<CounterResponse> Increment(ICounterService counterService)
    {
        var newValue = counterService.Increment();
        return TypedResults.Ok(new CounterResponse(newValue));
    }
}
