using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Http.HttpResults;
using TheButton.Api.Features.V2.Counter;
using TheButton.Application.Counter.V2.Increment;


namespace TheButton.Api.UnitTests.Features.V2.Counter;

[TestClass]
public class CounterEndpointsTests
{
    [TestMethod]
    public void Increment_ReturnsOk_WithNewValue()
    {
        // Arrange
        var mockService = new Mock<ICounterService>();
        var expectedValue = 99;
        mockService.Setup(s => s.Increment()).Returns(expectedValue);
        var handler = new IncrementHandler(mockService.Object);

        // Act
        var result = CounterEndpoints.Increment(handler);

        // Assert
        Assert.IsInstanceOfType(result, typeof(Ok<CounterResponse>));
        var okResult = (Ok<CounterResponse>)result;
        Assert.IsNotNull(okResult.Value);
        Assert.AreEqual(expectedValue, okResult.Value.Value);
        
        mockService.Verify(s => s.Increment(), Times.Once);
    }
}
