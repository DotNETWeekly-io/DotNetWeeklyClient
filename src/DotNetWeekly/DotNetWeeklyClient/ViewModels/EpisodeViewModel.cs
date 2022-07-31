using CommunityToolkit.Mvvm.ComponentModel;
using DotNetWeeklyClient.Services;
using Markdig;

namespace DotNetWeeklyClient.ViewModels
{
    [QueryProperty("EpisodeId", "EpisodeId")]
    public partial class EpisodeViewModel : ObservableObject
    {
        IConnectivity connectivity;

        IEpisodeService episodeService;

        public EpisodeViewModel(IConnectivity connectivity, IEpisodeService episodeService)
        {
            this.connectivity = connectivity;
            this.episodeService = episodeService;
        }

        [ObservableProperty]
        string episodeId;

        [ObservableProperty]
        string episodeContent;

        public async Task UpdateEpisode()
        {
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("错误", "无网络链接", "OK");
                return;
            }

            var episode = await this.episodeService.GetEpisode(episodeId, default);
            if (episode == null)
            {
                await Shell.Current.DisplayAlert("错误", "无法找到该内容", "OK");
                return;
            }

            var episodeHtml = Markdown.ToHtml(episode.Content);
            EpisodeContent = episodeHtml;
        }
    }
}
