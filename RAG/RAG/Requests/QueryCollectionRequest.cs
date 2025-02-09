using Application.Abstractions;
using ChromaDB.Client.Models;
using RAG.Repository;
using RAG.Services.Embedding;

namespace RAG.Requests
{
    public class QueryCollectionRequest
    {
        public string CollectionName { get; set; }
        public int Nresults { get; set; }
        public List<string> Prompts { get; set; }

        public IEmbeddingGeneratorFactory EmbeddingGeneratorFactory { get; set; }
        public IEmbeddingsRepository EmbeddingsRepository { get; set; }
        public ICollectionsRepository CollectionsRepository { get; set; }
    }
}
