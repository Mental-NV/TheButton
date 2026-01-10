using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TheButton.Mobile.Core;

namespace TheButton.Mobile.Core.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ICounterApiClient _apiClient;

    [ObservableProperty]
    private int _value;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    public bool IsNotBusy => !IsBusy;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public MainViewModel(ICounterApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [RelayCommand]
    private async Task ClickAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            Value = await _apiClient.IncrementAsync();
        }
        catch (Exception)
        {
            ErrorMessage = "Something went wrong. Please try again.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
