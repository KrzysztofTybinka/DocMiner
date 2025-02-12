

using Domain.Abstractions;

namespace Infrastructure.Services.LanguageModelServices
{
    public static class LanguageModelErrors
    {
        public static Error CouldntProcess(string service, string message) => Error.ExternalServiceFailure(
            "LanguageModelErrors.CouldntProcess",
            $"A service {service} faild while processing your request.\n" +
            $"Error message: {message}"
        );

        public static Error EmptyResponse => Error.Failure(
            "LanguageModelErrors.EmptyResponse",
            "Language model service response was empty."
        );
    }
}
