using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Http.HttpResults;
using TheButton.Api.Features.V2.Counter;
using TheButton.Services;

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

        // Act
        var result = CounterEndpoints.Increment(mockService.Object);

        // Assert
        Assert.IsInstanceOfType(result, typeof(Ok<CounterResponse>));
        var okResult = (Ok<CounterResponse>)result;
        Assert.IsNotNull(okResult.Value);
        Assert.AreEqual(expectedValue, okResult.Value.Value);
        
        mockService.Verify(s => s.Increment(), Times.Once);
    }
}
