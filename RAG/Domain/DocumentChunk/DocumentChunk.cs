using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DocumentChunk
{
    public class DocumentChunk
    {
        public string Id { get; }
        public Embedding.Embedding Embedding { get; }
        public string FromDocument { get; }

        private DocumentChunk(string id, Embedding.Embedding embedding, string fromDocument)
        {
            Id = id;
            Embedding = embedding;
            FromDocument = fromDocument;
        }

        public static Result<DocumentChunk> Create(string id, Embedding.Embedding embedding, string fromDocument)
        {
            if (string.IsNullOrEmpty(id))
                return Result<DocumentChunk>.Failure(DocumentChunkErrors.IdEmpty);

            if (embedding == null)
                return Result<DocumentChunk>.Failure(DocumentChunkErrors.EmbeddingNull);

            if (string.IsNullOrEmpty(fromDocument))
                return Result<DocumentChunk>.Failure(DocumentChunkErrors.FromDocumentEmpty);

            return Result<DocumentChunk>.Success(new DocumentChunk(id, embedding, fromDocument));
        }
    }
}
