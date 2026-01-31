namespace TheButton.Application.Counter.V3.GetUser;

/// <summary>
/// Query to retrieve global and user-specific counter values.
/// </summary>
/// <param name="UserId">The user identifier.</param>
public record GetUserCountersQuery(Guid UserId);
