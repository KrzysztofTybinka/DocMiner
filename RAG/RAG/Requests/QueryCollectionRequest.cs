using Application.Abstractions;
using ChromaDB.Client.Models;
using RAG.Repository;

namespace RAG.Requests
{
    public class QueryCollectionRequest
    {
        public string CollectionName { get; set; }
        public int Nresults { get; set; }
        public string Prompt { get; set; }

        public IEmbeddingGeneratorFactory EmbeddingGeneratorFactory { get; set; }
        public IEmbeddingsRepository EmbeddingsRepository { get; set; }
        public ICollectionsRepository CollectionsRepository { get; set; }
    }
}
