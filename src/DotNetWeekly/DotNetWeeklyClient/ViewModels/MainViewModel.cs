using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DotNetWeeklyClient.Models;
using DotNetWeeklyClient.Services;
using System.Collections.ObjectModel;

namespace DotNetWeeklyClient.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private IConnectivity connectivity;

        private IEpisodeService episodeService;

        public MainViewModel(IConnectivity connectivity, IEpisodeService episodeService)
        {
            this.connectivity = connectivity;
            this.episodeService = episodeService;
        }

        public async Task Init()
        {
            var summaries = await this.episodeService.GetEpisodeSummaries(default);
            this.EpisodeSummaries = new ObservableCollection<EpisodeSummary>(summaries);
        }

        [ObservableProperty]
        ObservableCollection<EpisodeSummary> episodeSummaries;

        [ICommand]
        async Task Tap(string s)
        {
            await Shell.Current.GoToAsync($"{nameof(EpisodePage)}?EpisodeId={s}");
        }
    }
}
