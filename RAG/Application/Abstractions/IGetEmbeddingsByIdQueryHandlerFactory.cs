
using Application.Abstractions;
using Domain.Abstractions;

namespace Infrastructure.Abstractions
{
    public interface IGetEmbeddingsByIdQueryHandlerFactory
    {
        Task<Result<IGetEmbeddingsByIdQueryHandler>> CreateHandlerAsync(string collectionName);
    }
}
