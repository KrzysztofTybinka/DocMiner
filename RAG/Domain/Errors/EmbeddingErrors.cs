using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Errors
{
    public static class EmbeddingErrors
    {
        public static readonly Error NullVector = new(
            "Embedding.NullVector",
            "Embedding vector cannot be null."
);

        public static readonly Error EmptyVector = new(
            "Embedding.EmptyVector",
            "Embedding vector cannot be empty."
        );

        public static readonly Error NullOrEmptyChunk = new(
            "Embedding.NullOrEmptyChunk",
            "Chunk text cannot be null or empty."
        );
    }
}
