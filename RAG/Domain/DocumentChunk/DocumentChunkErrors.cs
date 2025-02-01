using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DocumentChunk
{
    public static class DocumentChunkErrors
    {
        public static readonly Error IdEmpty = new(
            "DocumentChunk.EmptyId",
            "Id cannot be empty."
        );

        public static readonly Error FromDocumentEmpty = new(
            "DocumentChunk.EmptyFromDocument",
            "FromDocument cannot be empty."
        );

        public static readonly Error EmbeddingNull = new(
            "DocumentChunk.NullEmbedding",
            "Embedding cannot be null."
        );
    }
}
