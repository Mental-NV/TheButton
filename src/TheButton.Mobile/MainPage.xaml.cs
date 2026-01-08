using TheButton.Mobile.Core.ViewModels;

namespace TheButton.Mobile;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
