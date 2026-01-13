using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheButton.Infrastructure.Counter.V2;

namespace TheButton.Api.UnitTests.Features.V2;

[TestClass]
public class CounterServiceTests
{
    private CounterService _service;

    [TestInitialize]
    public void Setup()
    {
        _service = new CounterService();
    }

    [TestMethod]
    public void Increment_IncreasesValue()
    {
        // Act
        var result1 = _service.Increment();
        var result2 = _service.Increment();

        // Assert
        Assert.AreEqual(1, result1);
        Assert.AreEqual(2, result2);
    }

    [TestMethod]
    public void GetCount_ReturnsCurrentValue()
    {
        // Arrange
        _service.Increment();

        // Act
        var count = _service.GetCount();

        // Assert
        Assert.AreEqual(1, count);
    }
}
