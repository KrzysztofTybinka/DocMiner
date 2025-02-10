
using Application.Queries;
using ChromaDB.Client;
using Domain.Abstractions;
using Infrastructure.Abstractions;
using Infrastructure.Queries;
using Infrastructure.Repositories.ChromaCollection;

namespace Infrastructure.Factories.Embeddings
{
    public class GetSimilarEmbeddingsQueryHandlerFactory : IGetSimilarEmbeddingsQueryHandlerFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ChromaConfigurationOptions _options;
        private readonly ChromaCollectionRepository _collectionRepository;

        public GetSimilarEmbeddingsQueryHandlerFactory(
            IHttpClientFactory httpClientFactory,
            ChromaConfigurationOptions options,
            ChromaCollectionRepository collectionRepository)
        {
            _httpClientFactory = httpClientFactory;
            _options = options;
            _collectionRepository = collectionRepository;
        }

        public async Task<Result<IGetSimilarEmbeddingsQueryHandler>> CreateHandlerAsync(string collectionName)
        {
            var collection = await _collectionRepository.GetDocumentCollection(collectionName);

            var httpClient = _httpClientFactory.CreateClient("ChromaDB");
            var client = new ChromaCollectionClient(collection, _options, httpClient);

            var embeddingRepository = new GetSimilarEmbeddingsQueryHandler(client);
            return Result<IGetSimilarEmbeddingsQueryHandler>.Success(embeddingRepository);
        }
    }
}
