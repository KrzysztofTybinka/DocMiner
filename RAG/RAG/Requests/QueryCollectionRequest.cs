using ChromaDB.Client.Models;
using RAG.Repository;
using RAG.Services.Embedding;

namespace RAG.Requests
{
    public class QueryCollectionRequest
    {
        public string CollectionName { get; set; }
        public int Nresults { get; set; }
        public string[] Prompts { get; set; }

        public EmbeddingServiceFactory EmbeddingServiceFactory { get; set; }
        public IEmbeddingsRepository EmbeddingsRepository { get; set; }
        public ICollectionsRepository CollectionsRepository { get; set; }
    }
}
