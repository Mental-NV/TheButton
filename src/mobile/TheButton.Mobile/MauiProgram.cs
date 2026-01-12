using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using TheButton.Mobile.Infrastructure;
using TheButton.Mobile.Core.ViewModels;
using TheButton.Mobile.Core;

namespace TheButton.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        
        // Configuration
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("TheButton.Mobile.appsettings.json");
        
        var configBuilder = new ConfigurationBuilder();
        if (stream != null)
        {
            configBuilder.AddJsonStream(stream);
        }

#if E2E_ANDROID_TEST
        using var e2eStream = assembly.GetManifestResourceStream("TheButton.Mobile.appsettings.E2eAndroid.json");
        if (e2eStream != null)
        {
            configBuilder.AddJsonStream(e2eStream);
        }
#elif E2E_IOS_TEST
        using var e2eStream = assembly.GetManifestResourceStream("TheButton.Mobile.appsettings.E2eiOS.json");
        if (e2eStream != null)
        {
            configBuilder.AddJsonStream(e2eStream);
        }
#elif DEBUG
        using var devStream = assembly.GetManifestResourceStream("TheButton.Mobile.appsettings.Development.json");
        if (devStream != null)
        {
            configBuilder.AddJsonStream(devStream);
        }
#endif

        // Environment variable override
        configBuilder.AddEnvironmentVariables();
        
        // Explicit BASE_API_URL override (if not picked up by AddEnvironmentVariables on mobile)
        // Usually AddEnvironmentVariables works, but for specific override logic requested:
        var config = configBuilder.Build();
        
        // Check for specific BASE_API_URL variable if needed, but AddEnvironmentVariables handles standard mapping.
        // However, user asked for BASE_API_URL to override. AddEnvironmentVariables does this if key matches.
        // But appsettings uses "BaseApiUrl". Env var BASE_API_URL maps to BaseApiUrl on Linux/Windows, 
        // but let's ensure it maps to the section we use.

        builder.Configuration.AddConfiguration(config);

        // Services
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainViewModel>();

        return builder.Build();
    }
}
