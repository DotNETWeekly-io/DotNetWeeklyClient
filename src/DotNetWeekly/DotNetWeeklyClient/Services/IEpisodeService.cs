using DotNetWeeklyClient.Models;

namespace DotNetWeeklyClient.Services
{
    public interface IEpisodeService
    {
        Task<IEnumerable<EpisodeSummary>> GetEpisodeSummaries(CancellationToken token);

        Task<Episode> GetEpisode(string id, CancellationToken token);
    }
}
