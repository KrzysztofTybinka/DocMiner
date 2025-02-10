

using Domain.Abstractions;

namespace Infrastructure.Queries.GetEmbeddingsById
{
    public static class GetEmbeddingsByIdQueryHandlerErrors
    {
        public static Error CollectionIsNull => Error.Failure(
            "EmbeddingRepositoryErrors.CollectionIsNull",
            "Failed when retrieving data from a collection."
        );
    }
}
