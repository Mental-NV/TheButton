using Moq;
using TheButton.Mobile.Core;
using TheButton.Mobile.Core.ViewModels;

namespace TheButton.Mobile.UnitTests;

[TestClass]
public class MainViewModelTests
{
    private Mock<ICounterApiClient> _mockApiClient;
    private MainViewModel _viewModel;

    [TestInitialize]
    public void Setup()
    {
        _mockApiClient = new Mock<ICounterApiClient>();
        _viewModel = new MainViewModel(_mockApiClient.Object);
    }

    [TestMethod]
    public async Task Click_Success_UpdatesValue_AndClearsError()
    {
        // Arrange
        _mockApiClient.Setup(x => x.IncrementAsync(It.IsAny<string>())).ReturnsAsync(5);
        _viewModel.ErrorMessage = "Old Error";

        // Act
        await _viewModel.ClickCommand.ExecuteAsync(null);

        // Assert
        Assert.AreEqual(5, _viewModel.Value);
        Assert.AreEqual(string.Empty, _viewModel.ErrorMessage);
        Assert.IsFalse(_viewModel.IsBusy);
    }

    [TestMethod]
    public async Task Click_Failure_SetsUserFacingError_AndClearsBusy()
    {
        // Arrange
        _mockApiClient.Setup(x => x.IncrementAsync(It.IsAny<string>())).ThrowsAsync(new Exception("API Error"));

        // Act
        await _viewModel.ClickCommand.ExecuteAsync(null);

        // Assert
        Assert.AreEqual(0, _viewModel.Value); // Value should not change (or stay default 0)
        Assert.IsFalse(string.IsNullOrEmpty(_viewModel.ErrorMessage));
        Assert.IsFalse(_viewModel.IsBusy);
    }
}
