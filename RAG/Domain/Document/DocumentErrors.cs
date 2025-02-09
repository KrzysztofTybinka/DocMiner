
using Domain.Abstractions;

namespace Domain.Document
{
    public static class DocumentErrors
    {
        public static Error NameEmpty => Error.Failure(
            "DocumentErrors.NameEmpty",
            "Document name cannot be empty."
        );

        public static Error EmbeddingsCollectionEmpty => Error.Failure(
            "DocumentErrors.EmbeddingsCollectionEmpty",
            "Document must contain at least one embedding."
        );
    }
}
