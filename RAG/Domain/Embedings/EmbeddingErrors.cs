using Domain.Abstractions;

namespace Domain.Embedings
{
    public static class EmbeddingErrors
    {
        public static Error TextEmpty => Error.Failure(
            "EmbeddingErrors.TextEmpty",
            "To create an embedding you need to provide text it is based on."
        );

        public static Error EmbedingEmpty => Error.Failure(
            "EmbeddingErrors.EmbedingEmpty",
            "Vector embedding was empty."
        );
    }
}
