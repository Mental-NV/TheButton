using TheButton.Application.Counter.V2.Increment;
using TheButton.Domain.Features.V2.Counter;

namespace TheButton.Api.Features.V2.Counter;

public static class CounterEndpoints
{
    public static RouteGroupBuilder MapV2CounterEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/", Increment);
        return group;
    }

    public static Microsoft.AspNetCore.Http.HttpResults.Ok<CounterResponse> Increment(IncrementHandler handler)
    {
        var result = handler.Handle(new IncrementCommand());
        return TypedResults.Ok(new CounterResponse(result.Value));
    }
}
