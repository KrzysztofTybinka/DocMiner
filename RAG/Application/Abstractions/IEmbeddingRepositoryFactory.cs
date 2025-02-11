

using Domain.Abstractions;
using Domain.Embedings;

namespace Infrastructure.Abstractions
{
    public interface IEmbeddingRepositoryFactory
    {
        /// <summary>
        /// Creates an instance of IEmbeddingRepository configured for the specified collection.
        /// </summary>
        Task<Result<IEmbeddingRepository>> CreateRepositoryAsync(string collectionName);
    }
}
