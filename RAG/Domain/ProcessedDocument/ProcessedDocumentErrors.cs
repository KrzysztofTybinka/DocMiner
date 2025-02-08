using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ProcessedDocument
{
    public static class ProcessedDocumentError
    {
        public static Error NameEmpty => Error.NotFound(
            "ProcessedDocumentError.NameEmpty",
            "Document name cannot be empty."
        );

        public static Error NoContent => Error.Failure(
            "ProcessedDocumentError.NoContent",
            "Document was empty."
        );
    }
}
