using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using TheButton.Mobile.Services;

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

        // Load configuration from appsettings*.json copied to output folder
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
#if DEBUG
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
#endif
            .Build();

        var baseApiUrl = configuration["BaseApiUrl"] ?? throw new InvalidOperationException("BaseApiUrl is not configured.");
        builder.Services.AddSingleton(new ApiConfig { BaseUrl = baseApiUrl });

        // Register HttpClientFactory + typed service
        builder.Services.AddHttpClient<IButtonService, ButtonService>((sp, client) =>
        {
            var cfg = sp.GetRequiredService<ApiConfig>();
            client.BaseAddress = new Uri(cfg.BaseUrl, UriKind.Absolute);
        });

		return builder.Build();
	}

    private sealed class ApiConfig
    {
        public string BaseUrl { get; set; } = "";
    }
}
