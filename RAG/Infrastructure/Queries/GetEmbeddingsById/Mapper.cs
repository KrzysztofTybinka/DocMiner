
using Application.Responses;
using ChromaDB.Client.Models;
using Domain.Abstractions;
using Domain.Embedings;

namespace Infrastructure.Queries.GetEmbeddingsById
{
    public static class Mapper
    {
        public static Result<List<GetEmbeddingsByIdResponse>> FromChromaCollectionEntryToEmbeddings(this List<ChromaCollectionEntry> collectionEntry)
        {
            var embeddings = new List<GetEmbeddingsByIdResponse>();

            foreach (ChromaCollectionEntry entry in collectionEntry)
            {
                entry.Metadata!.TryGetValue("source", out object? source);

                var embeddingResponse = new GetEmbeddingsByIdResponse()
                {
                    Id = new Guid(entry.Id),
                    Text = entry.Document ?? "",
                    Source = source as string ?? string.Empty
                };

                embeddings.Add(embeddingResponse);
            }

            return Result<List<GetEmbeddingsByIdResponse>>.Success(embeddings);
        }
    }
}
