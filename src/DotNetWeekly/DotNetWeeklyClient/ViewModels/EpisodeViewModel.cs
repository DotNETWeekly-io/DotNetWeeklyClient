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

        private static readonly string episodeformat1 = @"
         <html>
            <head>
              <style>
                    .episodePage {
                         padding-left: 5px;
                         padding-bottom: 60px;
                         text-align: left;
                         margin-left: 1%;
                         margin-right: 1%;
                    }
                    h1 {
                        text-align: center;
                    }
                    img {
                        max-width: 60%;
                        display: block;
                        margin-left: auto;
                        margin-right: auto;
                    }
                    pre {
                        color: black;
                        background: rgb(245, 242, 240);
                        text-shadow: white 0px 1px;
                        font-family: Consolas, Monaco, ""Andale Mono"", ""Ubuntu Mono"", monospace;
                        font-size: 1em;
                        text-align: left;
                        white-space: pre;
                        word-spacing: normal;
                        word-break: normal;
                        overflow-wrap: normal;
                        line-height: 1.5;
                        tab-size: 4;
                        hyphens: none;
                        padding: 1em;
                        margin: 0.5em 0px;
                        overflow: auto;
                    }
                </style>
                </head>
                <body>
                    <div class=""episodePage"">";

        private static readonly string episodeformat2 = @"
                    </div>
                </body>
            </html>";

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
            episodeHtml = $"{episodeformat1}{episodeHtml}{episodeformat2}";
            EpisodeContent = episodeHtml;
        }
    }
}
