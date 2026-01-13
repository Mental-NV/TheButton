using TheButton.Api.Abstractions;
using TheButton.Application.Counter.V2.Increment;

namespace TheButton.Api.Features.V2.Counter;

public class CounterEndpoints : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/counter")
            .WithTags("Counter");

        group.MapPost("", Increment);
    }

    public static Microsoft.AspNetCore.Http.HttpResults.Ok<CounterResponse> Increment(IncrementHandler handler)
    {
        var result = handler.Handle(new IncrementCommand());
        return TypedResults.Ok(new CounterResponse(result.Value));
    }
}
