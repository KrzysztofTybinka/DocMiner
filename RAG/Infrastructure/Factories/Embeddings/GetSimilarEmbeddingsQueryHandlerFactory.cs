
using Application.Queries;
using Domain.Abstractions;
using Infrastructure.Abstractions;
using Infrastructure.Queries.GetSimilarEmbeddings;

namespace Infrastructure.Factories.Embeddings
{
    public class GetSimilarEmbeddingsQueryHandlerFactory : IGetSimilarEmbeddingsQueryHandlerFactory
    {
        private readonly ChromaCollectionClientFactory _chromaCollectionClientFactory;

        public GetSimilarEmbeddingsQueryHandlerFactory(
            ChromaCollectionClientFactory chromaCollectionClientFactory)
        {
            _chromaCollectionClientFactory = chromaCollectionClientFactory;
        }

        public async Task<Result<IGetSimilarEmbeddingsQueryHandler>> CreateHandlerAsync(string collectionName)
        {
            var clientResult = await _chromaCollectionClientFactory.CreateClientAsync(collectionName);
            if (!clientResult.IsSuccess)
            {
                return Result<IGetSimilarEmbeddingsQueryHandler>.Failure(clientResult.Error);
            }

            var handler = new GetSimilarEmbeddingsQueryHandler(clientResult.Data);
            return Result<IGetSimilarEmbeddingsQueryHandler>.Success(handler);
        }
    }
}
