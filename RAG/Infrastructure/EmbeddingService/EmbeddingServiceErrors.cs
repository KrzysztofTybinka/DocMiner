
using Domain.Abstractions;

namespace Infrastructure.EmbeddingService
{
    public static class EmbeddingServiceErrors
    {
        public static Error CouldntProcess(string service, string message) => Error.ExternalServiceFailure(
            "EmbeddingServiceErrors.CouldntProcess",
            $"A service {service} faild while processing your request.\n" +
            $"Error message: {message}"
        );

        public static Error EmptyResponse => Error.Failure(
            "EmbeddingServiceErrors.EmptyResponse",
            "Embedding service response was empty."
        );
    }
}
