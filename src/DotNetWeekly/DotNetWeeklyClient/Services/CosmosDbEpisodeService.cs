using DotNetWeeklyClient.Models;
using DotNetWeeklyClient.Options;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace DotNetWeeklyClient.Services
{
    public class CosmosDbEpisodeService : IEpisodeService
    {
        private readonly CosmosDbOptions _options;

        public CosmosDbEpisodeService(IOptions<CosmosDbOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public async Task<Episode> GetEpisode(string id, CancellationToken token)
        {
            var container = await GetOrCreateContainer(false, token);
            PartitionKey key = new(id);
            return await container.ReadItemAsync<Episode>(id.ToString(), key);
        }

        public async Task<IEnumerable<EpisodeSummary>> GetEpisodeSummaries(CancellationToken token)
        {
            var container = await GetOrCreateContainer(true, token);
            var query = new QueryDefinition("SELECT * FROM c");
            var queryIterator = container.GetItemQueryIterator<EpisodeSummary>(query);
            var episodeSummary = new List<EpisodeSummary>();
            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                foreach (var item in response)
                {
                    episodeSummary.Add(item);
                }
            }
            return episodeSummary;
        }

        private async Task<Container> GetOrCreateContainer(bool isSummary, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(_options.EndPoint, nameof(_options.EndPoint));
            ArgumentNullException.ThrowIfNull(_options.PrimaryReadOnlyKey, nameof(_options.PrimaryReadOnlyKey));
            ArgumentNullException.ThrowIfNull(_options.DatabaseName, nameof(_options.DatabaseName));
            ArgumentNullException.ThrowIfNull(_options.EpisodeSummaryContainer, nameof(_options.EpisodeSummaryContainer));
            ArgumentNullException.ThrowIfNull(_options.EpisodeContainer, nameof(_options.EpisodeContainer));
            CosmosClient client = new CosmosClient(_options.EndPoint, _options.PrimaryReadOnlyKey); ;
            Database database = await client.CreateDatabaseIfNotExistsAsync(_options.DatabaseName, cancellationToken: token);
            Container container = await database.CreateContainerIfNotExistsAsync(isSummary ? _options.EpisodeSummaryContainer : _options.EpisodeContainer, "/id", cancellationToken: token);
            return container;
        }
    }
}
