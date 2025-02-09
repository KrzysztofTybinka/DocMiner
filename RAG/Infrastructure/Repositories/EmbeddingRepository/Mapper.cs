

using ChromaDB.Client.Models;
using Domain.Abstractions;
using Domain.Document;
using Domain.Embedings;
using System.Collections.Generic;

namespace Infrastructure.Repositories.DocumentRepository
{
    public static class Mapper
    {
        public static ChromaDocumentAddRequest EmbeddingsToChromaDbAddRequest(this List<Embedding> document)
        {
            var request = new ChromaDocumentAddRequest();

            request.Vectors = document
                .Select(e => new ReadOnlyMemory<float>(e.TextEmbedding.ToArray()))
                .ToList();

            request.TextContent = document
                .Select(e => e.Text)
                .ToList();

            request.Ids = document
                .Select(e => e.Id.ToString())
                .ToList();

            request.Metadatas = document
                .Select(e =>
                {
                    var source = e.Details?.Source;
                    return !string.IsNullOrEmpty(source)
                        ? new Dictionary<string, object> { { "source", source } }
                        : new Dictionary<string, object>(); // Return an empty dictionary if null
                })
                .ToList();


            return request;
        }

        public static Result<List<Embedding>> FromChromaCollectionQueryEntryToEmbeddings(this List<ChromaCollectionQueryEntry> queryEntry)
        {
            var embeddings = new List<Embedding>();

            foreach (ChromaCollectionQueryEntry entry in queryEntry)
            {
                var embedding = Embedding.Create(
                    new Guid(entry.Id),
                    entry.Document ?? "",
                    entry.Embeddings?.ToArray() ?? Array.Empty<float>());

                if (!embedding.IsSuccess)
                    return Result<List<Embedding>>.Failure(embedding.Error);

                embeddings.Add(embedding.Data);
            }

            return Result<List<Embedding>>.Success(embeddings);
        }

        public static Result<List<Embedding>> FromChromaCollectionEntryToEmbeddings(this List<ChromaCollectionEntry> collectionEntry)
        {
            var embeddings = new List<Embedding>();

            foreach (ChromaCollectionEntry entry in collectionEntry)
            {
                var embedding = Embedding.Create(
                    new Guid(entry.Id),
                    entry.Document ?? "",
                    entry.Embeddings?.ToArray() ?? Array.Empty<float>());

                if (!embedding.IsSuccess)
                    return Result<List<Embedding>>.Failure(embedding.Error);

                embeddings.Add(embedding.Data);
            }

            return Result<List<Embedding>>.Success(embeddings);
        }
    }

    public class ChromaDocumentAddRequest
    {
        public List<ReadOnlyMemory<float>> Vectors { get; set; }
        public List<string> TextContent { get; set; }
        public List<string> Ids { get; set; }
        public List<Dictionary<string, object>> Metadatas { get; set; }
    }
}
