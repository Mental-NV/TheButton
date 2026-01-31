using TheButton.Mobile.Core.ViewModels;

namespace TheButton.Mobile;

/// <summary>
/// Main page displaying the counter and actions.
/// </summary>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainPage"/> class.
    /// </summary>
    /// <param name="viewModel">The main view model.</param>
    public MainPage(MainViewModel viewModel)
    {
        this.InitializeComponent();
        this.BindingContext = viewModel;
    }

    /// <inheritdoc />
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (this.BindingContext is MainViewModel viewModel)
        {
            await viewModel.InitializeAsync().ConfigureAwait(true);
        }
    }
}
