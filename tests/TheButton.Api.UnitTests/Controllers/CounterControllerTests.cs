using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TheButton.Controllers;
using TheButton.Services;
using TheButton.Api.Models;

namespace TheButton.Api.UnitTests.Controllers;

[TestClass]
public class CounterControllerTests
{
    private Mock<ICounterService> _mockCounterService = null!;
    private CounterController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockCounterService = new Mock<ICounterService>();
        _controller = new CounterController(_mockCounterService.Object);
    }

    [TestMethod]
    public void Increment_ReturnsOkResult_WithCorrectValue()
    {
        // Arrange
        int expectedValue = 42;
        _mockCounterService.Setup(s => s.Increment()).Returns(expectedValue);

        // Act
        var result = _controller.Increment();

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        
        var data = okResult.Value as CounterResponse;
        Assert.IsNotNull(data);
        Assert.AreEqual(expectedValue, data.Value);
    }

    [TestMethod]
    public void Increment_CallsIncrementOnService()
    {
        // Act
        _controller.Increment();

        // Assert
        _mockCounterService.Verify(s => s.Increment(), Times.Once);
    }
}
