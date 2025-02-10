using Infrastructure.Abstractions;

namespace RAG.Requests
{
    public class DeleteEmbeddingsRequest
    {
        public IEmbeddingRepositoryFactory EmbeddingsRepositoryFactory { get; set; }
        public string CollectionName { get; set; }
        public string[]? Ids { get; set; }
    }
}
