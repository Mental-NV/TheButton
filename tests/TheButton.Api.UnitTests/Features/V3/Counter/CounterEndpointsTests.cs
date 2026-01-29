using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using TheButton.Api.Features.V3.Counter;
using TheButton.Application.Abstractions;
using TheButton.Application.Counter.V3.GetGlobal;
using TheButton.Application.Counter.V3.GetUser;
using TheButton.Application.Counter.V3.Increment;
using TheButton.Domain.Features.V3.Counter;

namespace TheButton.Api.UnitTests.Features.V3.Counter;

[TestClass]
public class CounterEndpointsTests
{
    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public async Task HandleGlobalIncrementAsync_MissingIdempotencyKey_GeneratesKey(string? idempotencyKey)
    {
        var counterWriterMock = new Mock<ICounterWriter>();
        string? capturedKey = null;
        Guid? capturedUserId = null;
        counterWriterMock
            .Setup(writer => writer.IncrementAsync(
                It.IsAny<string>(),
                It.IsAny<Guid?>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, Guid?, CancellationToken>((key, userId, _) =>
            {
                capturedKey = key;
                capturedUserId = userId;
            })
            .ReturnsAsync(new IncrementResult(10, 4));

        var handler = new IncrementHandler(counterWriterMock.Object);
        var result = await CounterEndpoints.HandleGlobalIncrementAsync(idempotencyKey, handler, CancellationToken.None);

        var okResult = result as Ok<CounterResponse>;
        Assert.IsNotNull(okResult);
        Assert.IsNotNull(okResult.Value);
        Assert.AreEqual(10, okResult.Value.Value);
        Assert.AreEqual(4, okResult.Value.UserValue);
        Assert.IsNotNull(capturedKey);
        Assert.IsTrue(Guid.TryParse(capturedKey, out _));
        Assert.IsNull(capturedUserId);
        counterWriterMock.Verify(
            writer => writer.IncrementAsync(
                It.IsAny<string>(),
                null,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task HandleUserIncrementAsync_UsesProvidedKeyAndUserId()
    {
        var counterWriterMock = new Mock<ICounterWriter>();
        var idempotencyKey = "test-key";
        var userId = Guid.NewGuid();

        counterWriterMock
            .Setup(writer => writer.IncrementAsync(
                idempotencyKey,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new IncrementResult(12, 3));

        var handler = new IncrementHandler(counterWriterMock.Object);
        var result = await CounterEndpoints.HandleUserIncrementAsync(
            idempotencyKey,
            userId,
            handler,
            CancellationToken.None);

        var okResult = result as Ok<CounterResponse>;
        Assert.IsNotNull(okResult);
        Assert.IsNotNull(okResult.Value);
        Assert.AreEqual(12, okResult.Value.Value);
        Assert.AreEqual(3, okResult.Value.UserValue);

        counterWriterMock.Verify(
            writer => writer.IncrementAsync(
                idempotencyKey,
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task HandleGlobalReadAsync_ReturnsGlobalValue()
    {
        var repositoryMock = new Mock<ICounterReadRepository>();
        repositoryMock
            .Setup(repository => repository.GetGlobalValueAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(42);

        var handler = new GetGlobalQueryHandler(repositoryMock.Object);
        var result = await CounterEndpoints.HandleGlobalReadAsync(handler, CancellationToken.None);

        var okResult = result as Ok<CounterResponse>;
        Assert.IsNotNull(okResult);
        Assert.IsNotNull(okResult.Value);
        Assert.AreEqual(42, okResult.Value.Value);
        Assert.IsNull(okResult.Value.UserValue);

        repositoryMock.Verify(repository => repository.GetGlobalValueAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task HandleUserReadAsync_ReturnsGlobalAndUserValues()
    {
        var repositoryMock = new Mock<ICounterReadRepository>();
        var userId = Guid.NewGuid();

        repositoryMock
            .Setup(repository => repository.GetGlobalValueAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(100);
        repositoryMock
            .Setup(repository => repository.GetUserValueAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(25);

        var handler = new GetUserCountersQueryHandler(repositoryMock.Object);
        var result = await CounterEndpoints.HandleUserReadAsync(userId, handler, CancellationToken.None);

        var okResult = result as Ok<CounterResponse>;
        Assert.IsNotNull(okResult);
        Assert.IsNotNull(okResult.Value);
        Assert.AreEqual(100, okResult.Value.Value);
        Assert.AreEqual(25, okResult.Value.UserValue);

        repositoryMock.Verify(repository => repository.GetGlobalValueAsync(It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repository => repository.GetUserValueAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
