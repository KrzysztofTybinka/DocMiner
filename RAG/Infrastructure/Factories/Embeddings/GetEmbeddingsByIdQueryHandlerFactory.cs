using Application.Queries;
using Domain.Abstractions;
using Infrastructure.Abstractions;
using Infrastructure.Queries.GetEmbeddingsById;

namespace Infrastructure.Factories.Embeddings
{
    public class GetEmbeddingsByIdQueryHandlerFactory : IGetEmbeddingsByIdQueryHandlerFactory
    {
        private readonly ChromaCollectionClientFactory _chromaCollectionClientFactory;

        public GetEmbeddingsByIdQueryHandlerFactory(
            ChromaCollectionClientFactory chromaCollectionClientFactory)
        {
            _chromaCollectionClientFactory = chromaCollectionClientFactory;
        }

        public async Task<Result<IGetEmbeddingsByIdQueryHandler>> CreateHandlerAsync(string collectionName)
        {
            var clientResult = await _chromaCollectionClientFactory.CreateClientAsync(collectionName);
            if (!clientResult.IsSuccess)
            {
                return Result<IGetEmbeddingsByIdQueryHandler>.Failure(clientResult.Error);
            }

            var handler = new GetEmbeddingsByIdQueryHandler(clientResult.Data);
            return Result<IGetEmbeddingsByIdQueryHandler>.Success(handler);
        }
    }
}
