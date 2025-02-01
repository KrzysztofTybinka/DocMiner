using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Document
{
    public static class DocumentErrors
    {
        public static readonly Error NameEmpty = new(
            "DocumentChunk.NameEmpty",
            "Name cannot be empty."
        );

        public static readonly Error StorageLocationEmpty = new(
            "DocumentChunk.StorageLocationEmpty",
            "Storage location cannot be empty."
        );
    }
}
