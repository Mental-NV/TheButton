using Moq;
using TheButton.Application.Abstractions;
using TheButton.Application.Counter.V3.Increment;

namespace TheButton.Api.UnitTests.Features.V3.Counter;

[TestClass]
public class IncrementHandlerTests
{
    private readonly Mock<ICounterWriter> _counterWriterMock;
    private readonly IncrementHandler _handler;

    public IncrementHandlerTests()
    {
        _counterWriterMock = new Mock<ICounterWriter>();
        _handler = new IncrementHandler(_counterWriterMock.Object);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task Handle_NullCommand_ThrowsArgumentNullException()
    {
        await _handler.Handle(null!);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task Handle_EmptyIdempotencyKey_ThrowsArgumentException()
    {
        var command = new IncrementCommand("", null);
        await _handler.Handle(command);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task Handle_WhitespaceIdempotencyKey_ThrowsArgumentException()
    {
        var command = new IncrementCommand("   ", null);
        await _handler.Handle(command);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task Handle_EmptyUserId_ThrowsArgumentException()
    {
        var command = new IncrementCommand("key", Guid.Empty);
        await _handler.Handle(command);
    }

    [TestMethod]
    public async Task Handle_ValidCommand_CallsCounterWriter()
    {
        // Arrange
        var idempotencyKey = "test-key";
        var userId = Guid.NewGuid();
        var command = new IncrementCommand(idempotencyKey, userId);

        _counterWriterMock.Setup(x => x.IncrementAsync(idempotencyKey, userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TheButton.Domain.Features.V3.Counter.IncrementResult(10, 1));

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.AreEqual(10, result.Value);
        Assert.AreEqual(1, result.UserValue);
        _counterWriterMock.Verify(x => x.IncrementAsync(idempotencyKey, userId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
