using DotNetWeeklyClient.ViewModels;

namespace DotNetWeeklyClient;

public partial class EpisodePage : ContentPage
{
	private EpisodeViewModel episodeViewModel;
	public EpisodePage(EpisodeViewModel vm)
	{
		InitializeComponent();
		episodeViewModel = vm;
		BindingContext = vm;
	}

    protected override async void OnAppearing()
	{
		await episodeViewModel.UpdateEpisode();
    }
}