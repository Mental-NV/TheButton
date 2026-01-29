using TheButton.Application.Counter.V3.Increment;

namespace TheButton.Api.UnitTests.Features.V3.Counter;

[TestClass]
public class IncrementCommandFactoryTests
{
    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Create_MissingIdempotencyKey_GeneratesNewKey(string? idempotencyKey)
    {
        var command = IncrementCommandFactory.Create(idempotencyKey, null);

        Assert.IsNotNull(command.IdempotencyKey);
        Assert.IsTrue(Guid.TryParse(command.IdempotencyKey, out _));
        Assert.IsNull(command.UserId);
    }

    [TestMethod]
    public void Create_WithValues_PreservesInputs()
    {
        var userId = Guid.NewGuid();
        var command = IncrementCommandFactory.Create("key", userId);

        Assert.AreEqual("key", command.IdempotencyKey);
        Assert.AreEqual(userId, command.UserId);
    }
}
