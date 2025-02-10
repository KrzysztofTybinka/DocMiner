using Application.Queries;
using ChromaDB.Client;
using Domain.Abstractions;
using Infrastructure.Abstractions;
using Infrastructure.Queries.GetEmbeddingsById;
using Infrastructure.Repositories.ChromaCollection;

namespace Infrastructure.Factories.Embeddings
{
    public class GetEmbeddingsByIdQueryHandlerFactory : IGetEmbeddingsByIdQueryHandlerFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ChromaConfigurationOptions _options;
        private readonly ChromaCollectionRepository _collectionRepository;

        public GetEmbeddingsByIdQueryHandlerFactory(
            IHttpClientFactory httpClientFactory,
            ChromaConfigurationOptions options,
            ChromaCollectionRepository collectionRepository)
        {
            _httpClientFactory = httpClientFactory;
            _options = options;
            _collectionRepository = collectionRepository;
        }

        public async Task<Result<IGetEmbeddingsByIdQueryHandler>> CreateHandlerAsync(string collectionName)
        {
            var collection = await _collectionRepository.GetDocumentCollection(collectionName);

            var httpClient = _httpClientFactory.CreateClient("ChromaDB");
            var client = new ChromaCollectionClient(collection, _options, httpClient);

            var embeddingRepository = new GetEmbeddingsByIdQueryHandler(client);
            return Result<IGetEmbeddingsByIdQueryHandler>.Success(embeddingRepository);
        }
    }
}
