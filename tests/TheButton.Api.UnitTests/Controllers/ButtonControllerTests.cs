using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TheButton.Controllers;
using TheButton.Services;
using TheButton.Api.Models;

namespace TheButton.Api.UnitTests.Controllers;

[TestClass]
public class ButtonControllerTests
{
    private Mock<ICounterService> _mockCounterService;
    private ButtonController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockCounterService = new Mock<ICounterService>();
        _controller = new ButtonController(_mockCounterService.Object);
    }

    [TestMethod]
    public void Click_ReturnsOkResult_WithCorrectValue()
    {
        // Arrange
        int expectedValue = 5;
        _mockCounterService.Setup(s => s.Increment()).Returns(expectedValue);

        // Act
        var result = _controller.Click();

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        
        var data = okResult.Value as CounterResponse;
        Assert.IsNotNull(data);
        Assert.AreEqual(expectedValue, data.Value);
    }

    [TestMethod]
    public void Click_CallsIncrementOnService()
    {
        // Act
        _controller.Click();

        // Assert
        _mockCounterService.Verify(s => s.Increment(), Times.Once);
    }
}
