
using Domain.Abstractions;

namespace Infrastructure.Repositories.DocumentRepository
{
    public class EmbeddingRepositoryErrors
    {
        public static Error CollectionIsNull => Error.Failure(
            "EmbeddingRepositoryErrors.CollectionIsNull",
            "Failed when retrieving data from a collection."
        );

        public static Error DeleteEmbeddingIdsNull => Error.Failure(
            "EmbeddingRepositoryErrors.DeleteEmbeddingIdsNull",
            "You need to specify embedding id that you want to delete."
        );
    }
}
