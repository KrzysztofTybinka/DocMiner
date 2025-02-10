using Domain.Abstractions;

namespace Infrastructure.Services.Ocr
{
    public static class OcrServiceErrors
    {
        public static Error CouldntProcess => Error.ExternalServiceFailure(
            "OcrServiceErrors.CouldntProcess",
            "Ocr service couldn't process your request."
        );

        public static Error EmptyResponse => Error.Failure(
            "OcrServiceErrors.EmptyResponse",
            "The response was empty."
        );
    }
}
