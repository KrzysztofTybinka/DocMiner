
using Application.Responses;
using ChromaDB.Client.Models;

namespace Infrastructure.Queries.GetSimilarEmbeddings
{
    public static class Mapper
    {
        public static List<GetSimilarEmbeddingsResponse> FromChromaCollectionQueryEntryToSimilarEmbeddingsResponse(this List<ChromaCollectionQueryEntry> queryEntry)
        {
            var embeddings = new List<GetSimilarEmbeddingsResponse>();

            foreach (ChromaCollectionQueryEntry entry in queryEntry)
            {
                entry.Metadata!.TryGetValue("source", out object? source);

                embeddings.Add(new GetSimilarEmbeddingsResponse()
                {
                    Id = new Guid(entry.Id),
                    Text = entry.Document ?? "",
                    Source = source as string ?? string.Empty,
                    Distance = entry.Distance,
                });
            }
            return embeddings;
        }
    }
}
