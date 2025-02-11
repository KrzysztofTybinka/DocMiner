using Application.Abstractions;
using Domain.Abstractions;

namespace Infrastructure.Abstractions
{
    public interface IGetSimilarEmbeddingsQueryHandlerFactory
    {
        /// <summary>
        /// Creates an instance of IGetSimilarEmbeddingsQueryHandler configured for the specified collection.
        /// </summary>
        Task<Result<IGetSimilarEmbeddingsQueryHandler>> CreateHandlerAsync(string collectionName);
    }
}
