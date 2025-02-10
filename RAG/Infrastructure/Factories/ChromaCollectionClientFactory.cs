
using ChromaDB.Client;
using Domain.Abstractions;
using Infrastructure.Repositories.ChromaCollection;

namespace Infrastructure.Factories
{
    public class ChromaCollectionClientFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ChromaConfigurationOptions _options;
        private readonly ChromaCollectionRepository _collectionRepository;

        public ChromaCollectionClientFactory(
            IHttpClientFactory httpClientFactory,
            ChromaConfigurationOptions options,
            ChromaCollectionRepository collectionRepository)
        {
            _httpClientFactory = httpClientFactory;
            _options = options;
            _collectionRepository = collectionRepository;
        }

        public async Task<Result<ChromaCollectionClient>> CreateClientAsync(string collectionName)
        {
            var collection = await _collectionRepository.GetDocumentCollection(collectionName);
            var httpClient = _httpClientFactory.CreateClient("ChromaDB");
            var client = new ChromaCollectionClient(collection, _options, httpClient);
            return Result<ChromaCollectionClient>.Success(client);
        }
    }
}
