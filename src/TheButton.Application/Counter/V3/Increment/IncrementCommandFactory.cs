using TheButton.Domain.Features.V3.Counter;

namespace TheButton.Application.Counter.V3.Increment;

public static class IncrementCommandFactory
{
    public static IncrementCommand Create(string? idempotencyKey, Guid? userId)
    {
        var normalizedKey = string.IsNullOrWhiteSpace(idempotencyKey)
            ? Guid.NewGuid().ToString()
            : idempotencyKey;

        return new IncrementCommand(normalizedKey, userId);
    }
}
