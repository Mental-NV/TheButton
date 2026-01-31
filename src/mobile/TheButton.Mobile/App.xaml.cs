namespace TheButton.Mobile;

/// <summary>
/// Application entry point for the mobile UI.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }

    /// <inheritdoc />
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
