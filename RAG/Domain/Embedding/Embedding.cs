using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Embedding
{
    public class Embedding
    {
        public IEnumerable<float> EmbeddingVector { get; }
        public string Chunk { get; }

        private Embedding(IEnumerable<float> embeddingVector, string chunk)
        {
            EmbeddingVector = embeddingVector;
            Chunk = chunk;
        }

        public static Result<Embedding> Create(IEnumerable<float> embeddingVector, string chunk)
        {
            if (embeddingVector == null)
                return Result<Embedding>.Failure(EmbeddingErrors.NullVector);

            if (!embeddingVector.Any())
                return Result<Embedding>.Failure(EmbeddingErrors.EmptyVector);

            if (string.IsNullOrEmpty(chunk))
                return Result<Embedding>.Failure(EmbeddingErrors.NullOrEmptyChunk);

            return Result<Embedding>.Success(new Embedding(embeddingVector, chunk));
        }
    }
}
