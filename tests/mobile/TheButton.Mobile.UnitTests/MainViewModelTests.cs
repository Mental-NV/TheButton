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
        _mockApiClient.Setup(x => x.IncrementAsync()).ReturnsAsync(5);
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
        _mockApiClient.Setup(x => x.IncrementAsync()).ThrowsAsync(new InvalidOperationException("API Error"));

        // Act
        await _viewModel.ClickCommand.ExecuteAsync(null);

        // Assert
        Assert.AreEqual(0, _viewModel.Value); // Value should not change (or stay default 0)
        Assert.IsFalse(string.IsNullOrEmpty(_viewModel.ErrorMessage));
        Assert.IsFalse(_viewModel.IsBusy);
    }

    [TestMethod]
    public async Task Initialize_Success_UpdatesValue_AndClearsError()
    {
        // Arrange
        _mockApiClient.Setup(x => x.GetAsync()).ReturnsAsync(7);
        _viewModel.ErrorMessage = "Old Error";

        // Act
        await _viewModel.InitializeAsync();

        // Assert
        Assert.AreEqual(7, _viewModel.Value);
        Assert.AreEqual(string.Empty, _viewModel.ErrorMessage);
        Assert.IsFalse(_viewModel.IsBusy);
    }

    [TestMethod]
    public async Task Initialize_Failure_SetsUserFacingError_AndClearsBusy()
    {
        // Arrange
        _mockApiClient.Setup(x => x.GetAsync()).ThrowsAsync(new InvalidOperationException("API Error"));

        // Act
        await _viewModel.InitializeAsync();

        // Assert
        Assert.AreEqual(0, _viewModel.Value);
        Assert.IsFalse(string.IsNullOrEmpty(_viewModel.ErrorMessage));
        Assert.IsFalse(_viewModel.IsBusy);
    }

    [TestMethod]
    public void Initialize_StartsWithIsInitializedFalse()
    {
        // Assert
        Assert.IsFalse(_viewModel.IsInitialized);
    }

    [TestMethod]
    public async Task Initialize_Success_SetsIsInitializedTrue()
    {
        // Arrange
        _mockApiClient.Setup(x => x.GetAsync()).ReturnsAsync(42);

        // Act
        await _viewModel.InitializeAsync();

        // Assert
        Assert.IsTrue(_viewModel.IsInitialized);
    }

    [TestMethod]
    public async Task Initialize_Failure_SetsIsInitializedTrue()
    {
        // Arrange
        _mockApiClient.Setup(x => x.GetAsync()).ThrowsAsync(new InvalidOperationException("API Error"));

        // Act
        await _viewModel.InitializeAsync();

        // Assert
        Assert.IsTrue(_viewModel.IsInitialized);
    }

    [TestMethod]
    public async Task Initialize_WhenAlreadyBusy_SkipsExecution_DoesNotSetIsInitialized()
    {
        // Arrange
        _viewModel.IsBusy = true;

        // Act
        await _viewModel.InitializeAsync();

        // Assert
        Assert.IsFalse(_viewModel.IsInitialized);
        _mockApiClient.Verify(x => x.GetAsync(), Times.Never);
    }

    [TestMethod]
    public async Task Initialize_MultipleCallsConcurrently_OnlyOneExecutes_IsInitializedSetOnce()
    {
        // Arrange
        var tcs = new TaskCompletionSource<int>();
        _mockApiClient.Setup(x => x.GetAsync()).Returns(tcs.Task);

        // Act - start first call
        var task1 = _viewModel.InitializeAsync();
        Assert.IsTrue(_viewModel.IsBusy);
        Assert.IsFalse(_viewModel.IsInitialized);

        // Try to start second call while first is in progress
        var task2 = _viewModel.InitializeAsync();
        
        // Complete the first call
        tcs.SetResult(99);
        await task1;

        // Assert
        Assert.IsFalse(_viewModel.IsBusy);
        Assert.IsTrue(_viewModel.IsInitialized);
        Assert.AreEqual(99, _viewModel.Value);
        // Second call should have returned immediately without executing
        _mockApiClient.Verify(x => x.GetAsync(), Times.Exactly(1));
    }

    [TestMethod]
    public async Task Initialize_SetsBusyFalseInFinallyAfterSuccess()
    {
        // Arrange
        _mockApiClient.Setup(x => x.GetAsync()).ReturnsAsync(15);

        // Act
        await _viewModel.InitializeAsync();

        // Assert
        Assert.IsFalse(_viewModel.IsBusy);
        Assert.IsTrue(_viewModel.IsInitialized);
    }

    [TestMethod]
    public async Task Initialize_SetsBusyFalseInFinallyAfterFailure()
    {
        // Arrange
        _mockApiClient.Setup(x => x.GetAsync()).ThrowsAsync(new InvalidOperationException("Network Error"));

        // Act
        await _viewModel.InitializeAsync();

        // Assert
        Assert.IsFalse(_viewModel.IsBusy);
        Assert.IsTrue(_viewModel.IsInitialized);
    }
}
