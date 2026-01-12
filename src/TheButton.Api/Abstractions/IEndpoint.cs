using Microsoft.AspNetCore.Routing;

namespace TheButton.Api.Abstractions;

public interface IEndpoint
{
    void Map(IEndpointRouteBuilder app);
}
