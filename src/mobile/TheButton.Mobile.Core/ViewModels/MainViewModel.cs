using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TheButton.Mobile.Core.ViewModels;

/// <summary>
/// View model for the main counter screen.
/// </summary>
/// <param name="apiClient">The counter API client.</param>
public partial class MainViewModel(ICounterApiClient apiClient)
    : ObservableObject
{
    private readonly ICounterApiClient _apiClient =
        apiClient ?? throw new ArgumentNullException(nameof(apiClient));

    [ObservableProperty]
    private int _value;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isInitialized;

    /// <summary>
    /// Gets a value indicating whether the view model is not busy.
    /// </summary>
    public bool IsNotBusy => !this.IsBusy;

    /// <summary>
    /// Initializes the view model by loading the current counter value.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InitializeAsync()
    {
        if (this.IsBusy)
        {
            return;
        }

        try
        {
            this.IsBusy = true;
            this.ErrorMessage = string.Empty;
            this.Value = await this._apiClient.GetAsync().ConfigureAwait(false);
        }
        catch (HttpRequestException)
        {
            this.ErrorMessage = "Something went wrong. Please try again.";
        }
        catch (TaskCanceledException)
        {
            this.ErrorMessage = "Request timed out. Please try again.";
        }
        catch (InvalidOperationException)
        {
            this.ErrorMessage = "Something went wrong. Please try again.";
        }
        finally
        {
            this.IsBusy = false;
            this.IsInitialized = true;
        }
    }

    [RelayCommand]
    private async Task ClickAsync()
    {
        if (this.IsBusy)
        {
            return;
        }

        try
        {
            this.IsBusy = true;
            this.ErrorMessage = string.Empty;
            this.Value = await this._apiClient.IncrementAsync().ConfigureAwait(false);
        }
        catch (HttpRequestException)
        {
            this.ErrorMessage = "Something went wrong. Please try again.";
        }
        catch (TaskCanceledException)
        {
            this.ErrorMessage = "Request timed out. Please try again.";
        }
        catch (InvalidOperationException)
        {
            this.ErrorMessage = "Something went wrong. Please try again.";
        }
        finally
        {
            this.IsBusy = false;
        }
    }
}
