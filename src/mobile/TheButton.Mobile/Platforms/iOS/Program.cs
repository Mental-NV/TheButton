using UIKit;

namespace TheButton.Mobile.Platforms.iOS;

/// <summary>
/// Program entry point for iOS.
/// </summary>
public static class Program
{
    /// <summary>
    /// Main entry point of the application.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    public static void Main(string[] args)
    {
        UIApplication.Main(args, principalClass: null, delegateClass: typeof(AppDelegate));
    }
}
