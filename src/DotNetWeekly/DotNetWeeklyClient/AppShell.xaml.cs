namespace DotNetWeeklyClient;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(EpisodePage), typeof(EpisodePage));
	}
}
