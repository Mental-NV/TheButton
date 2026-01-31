using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TheButton.Mobile.Core.ViewModels;
using TheButton.Mobile.Infrastructure;

namespace TheButton.Mobile;

/// <summary>
/// Configures the MAUI application.
/// </summary>
internal static class MauiProgram
{
    /// <summary>
    /// Creates the configured MAUI application instance.
    /// </summary>
    /// <returns>The configured <see cref="MauiApp"/>.</returns>
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();

        _ = builder.UseMauiApp<App>();
        _ = builder.ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });

#if DEBUG
        _ = builder.Logging.AddDebug();
#endif

        // Configuration
        var assembly = Assembly.GetExecutingAssembly();
        using Stream? stream = assembly.GetManifestResourceStream("TheButton.Mobile.appsettings.json");

        var configBuilder = new ConfigurationBuilder();
        if (stream is not null)
        {
            _ = configBuilder.AddJsonStream(stream);
        }

#if E2E_ANDROID_TEST
        using Stream? e2eStream = assembly.GetManifestResourceStream("TheButton.Mobile.appsettings.E2eAndroid.json");
        if (e2eStream is not null)
        {
            _ = configBuilder.AddJsonStream(e2eStream);
        }
#elif E2E_IOS_TEST
        using Stream? e2eStream = assembly.GetManifestResourceStream("TheButton.Mobile.appsettings.E2eiOS.json");
        if (e2eStream is not null)
        {
            _ = configBuilder.AddJsonStream(e2eStream);
        }
#elif DEBUG
        using Stream? devStream = assembly.GetManifestResourceStream("TheButton.Mobile.appsettings.Development.json");
        if (devStream is not null)
        {
            _ = configBuilder.AddJsonStream(devStream);
        }
#endif

        // Environment variable override
        _ = configBuilder.AddEnvironmentVariables();

        IConfigurationRoot config = configBuilder.Build();
        _ = builder.Configuration.AddConfiguration(config);

        // Services
        _ = builder.Services.AddInfrastructure(builder.Configuration);
        _ = builder.Services.AddSingleton<MainPage>();
        _ = builder.Services.AddSingleton<MainViewModel>();

        return builder.Build();
    }
}
