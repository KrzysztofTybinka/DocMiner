using ChromaDB.Client;
using Domain.Abstractions;
using Domain.Embedings;
using Infrastructure.Abstractions;
using Infrastructure.Repositories.ChromaCollection;
using Infrastructure.Repositories.DocumentRepository;

namespace Infrastructure.Factories.Embeddings
{
    public class EmbeddingRepositoryFactory : IEmbeddingRepositoryFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ChromaConfigurationOptions _options;
        private readonly ChromaCollectionRepository _collectionRepository;

        public EmbeddingRepositoryFactory(
            IHttpClientFactory httpClientFactory,
            ChromaConfigurationOptions options,
            ChromaCollectionRepository collectionRepository)
        {
            _httpClientFactory = httpClientFactory;
            _options = options;
            _collectionRepository = collectionRepository;
        }

        /// <summary>
        /// Returns a single collection, which is a repository for embeddings.
        /// </summary>
        /// <param name="collectionName">Name of a collection</param>
        /// <returns>Embedding repository</returns>
        public async Task<Result<IEmbeddingRepository>> CreateRepositoryAsync(string collectionName)
        {
            var collection = await _collectionRepository.GetDocumentCollection(collectionName);

            var httpClient = _httpClientFactory.CreateClient("ChromaDB");
            var client = new ChromaCollectionClient(collection, _options, httpClient);

            var embeddingRepository = new EmbeddingRepository(client);
            return Result<IEmbeddingRepository>.Success(embeddingRepository);
        }
    }

}
