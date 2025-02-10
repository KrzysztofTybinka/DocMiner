using Domain.Abstractions;
using Domain.Embedings;
using Infrastructure.Abstractions;
using Infrastructure.Repositories.DocumentRepository;

namespace Infrastructure.Factories.Embeddings
{
    public class EmbeddingRepositoryFactory : IEmbeddingRepositoryFactory
    {
        private readonly ChromaCollectionClientFactory _chromaCollectionClientFactory;

        public EmbeddingRepositoryFactory(
            ChromaCollectionClientFactory chromaCollectionClientFactory)
        {
            _chromaCollectionClientFactory = chromaCollectionClientFactory;
        }

        /// <summary>
        /// Returns a single collection, which is a repository for embeddings.
        /// </summary>
        /// <param name="collectionName">Name of a collection</param>
        /// <returns>Embedding repository</returns>
        public async Task<Result<IEmbeddingRepository>> CreateRepositoryAsync(string collectionName)
        {
            var clientResult = await _chromaCollectionClientFactory.CreateClientAsync(collectionName);
            if (!clientResult.IsSuccess)
            {
                return Result<IEmbeddingRepository>.Failure(clientResult.Error);
            }

            var handler = new EmbeddingRepository(clientResult.Data);
            return Result<IEmbeddingRepository>.Success(handler);
        }
    }

}
