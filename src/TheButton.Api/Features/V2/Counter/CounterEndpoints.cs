using TheButton.Api.Abstractions;

namespace TheButton.Api.Features.V2.Counter;

public class CounterEndpoints : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/counter")
            .WithTags("Counter");

        group.MapPost("", Increment);
    }

    public static Microsoft.AspNetCore.Http.HttpResults.Ok<CounterResponse> Increment(ICounterService counterService)
    {
        var newValue = counterService.Increment();
        return TypedResults.Ok(new CounterResponse(newValue));
    }
}
