using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ProcessedDocument
{
    public static class ProcessedDocumentErrors
    {
        public static readonly Error NameEmpty = new(
            "ProcessedDocument.Name",
            "Name cannot be empty."
        );

        public static readonly Error ContentEmpty = new(
            "ProcessedDocument.Content",
            "Content cannot be empty."
        );
    }
}
