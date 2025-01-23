using RAG.Repository;
using RAG.Services.Embedding;

namespace RAG.Requests
{
    public class QueryCollectionRequest
    {
        public string CollectionId { get; set; }
        public int Nresults { get; set; }

        public string[] Prompts { get; set; }

        public string[]? Keywords { get; set; } = null;

        public EmbeddingServiceFactory EmbeddingServiceFactory { get; set; }
        public IEmbeddingsRepository Repository { get; set; }
    }
}
