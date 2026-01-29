using Moq;
using TheButton.Application.Abstractions;
using TheButton.Application.Counter.V3.GetGlobal;
using TheButton.Application.Counter.V3.GetUser;

namespace TheButton.Api.UnitTests.Features.V3.Counter;

[TestClass]
public class GetCounterHandlerTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task GetGlobalCounterHandler_NullQuery_Throws()
    {
        var repositoryMock = new Mock<ICounterReadRepository>();
        var handler = new GetGlobalQueryHandler(repositoryMock.Object);

        await handler.Handle(null!, CancellationToken.None);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task GetUserCounterHandler_NullQuery_Throws()
    {
        var repositoryMock = new Mock<ICounterReadRepository>();
        var handler = new GetUserCountersQueryHandler(repositoryMock.Object);

        await handler.Handle(null!, CancellationToken.None);
    }

    [TestMethod]
    public async Task GetGlobalCounterHandler_ReturnsSnapshot()
    {
        var repositoryMock = new Mock<ICounterReadRepository>();
        repositoryMock
            .Setup(repository => repository.GetGlobalValueAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(15);

        var handler = new GetGlobalQueryHandler(repositoryMock.Object);

        var snapshot = await handler.Handle(new GetGlobalQuery(), CancellationToken.None);

        Assert.AreEqual(15, snapshot.Value);
        repositoryMock.Verify(repository => repository.GetGlobalValueAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task GetUserCounterHandler_ReturnsSnapshot()
    {
        var repositoryMock = new Mock<ICounterReadRepository>();
        var userId = Guid.NewGuid();

        repositoryMock
            .Setup(repository => repository.GetGlobalValueAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(21);
        repositoryMock
            .Setup(repository => repository.GetUserValueAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(8);

        var handler = new GetUserCountersQueryHandler(repositoryMock.Object);

        var snapshot = await handler.Handle(new GetUserCountersQuery(userId), CancellationToken.None);

        Assert.AreEqual(21, snapshot.GlobalValue);
        Assert.AreEqual(8, snapshot.UserValue);
        repositoryMock.Verify(repository => repository.GetGlobalValueAsync(It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repository => repository.GetUserValueAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
