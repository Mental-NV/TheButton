using TheButton.Mobile.Helpers;
using TheButton.Mobile.Services;

namespace TheButton.Mobile;

public partial class MainPage : ContentPage
{
    private readonly IButtonService _buttonService;

    public MainPage()
    {
        InitializeComponent();
        _buttonService = ServiceHelper.GetService<IButtonService>();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        try
        {
            CounterBtn.IsEnabled = false;
            CounterBtn.Text = "Calling API...";
            var value = await _buttonService.ClickAsync();
            CounterBtn.Text = $"Clicked {value} times";
        }
        catch
        {
            CounterBtn.Text = "Error calling API";
        }
        finally
        {
            CounterBtn.IsEnabled = true;
        }
    }
}
