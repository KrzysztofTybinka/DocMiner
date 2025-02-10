
using Domain.Abstractions;

namespace Infrastructure.Repositories.Collection
{
    public static class ChromaCollectionRepositoryErrors
    {
        public static Error CollectionNotExist(string collectionName) => Error.Failure(
            "ChromaCollectionRepositoryErrors.CollectionNotExist",
            $"A collection of a name: {collectionName} doesn't exist."
        );
    }
}
