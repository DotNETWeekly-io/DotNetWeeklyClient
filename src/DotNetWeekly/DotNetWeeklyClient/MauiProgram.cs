using DotNetWeeklyClient.Options;
using DotNetWeeklyClient.Services;
using DotNetWeeklyClient.ViewModels;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace DotNetWeeklyClient;

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

		var assembly = Assembly.GetExecutingAssembly();
		using var stream = assembly.GetManifestResourceStream("DotNetWeeklyClient.appsettings.json");
		var config = new ConfigurationBuilder()
			.AddJsonStream(stream)
			.Build();
		builder.Services.Configure<CosmosDbOptions>(config.GetSection("CosmosDb"));
		builder.Services.AddOptions();
		builder.Services.AddSingleton<IEpisodeService, CosmosDbEpisodeService>();
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainViewModel>();

		builder.Services.AddTransient<EpisodePage>();
		builder.Services.AddTransient<EpisodeViewModel>();
		return builder.Build();
	}
}
