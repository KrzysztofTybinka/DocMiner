using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.OcrService
{
    public static class OcrServiceErrors
    {
        public static Error CouldntProcess => Error.Failure(
            "OcrServiceErrors.CouldntProcess",
            "Ocr service couldn't process your request."
        );

        public static Error EmptyResponse => Error.Failure(
            "OcrServiceErrors.EmptyResponse",
            "The response was empty."
        );
    }
}
