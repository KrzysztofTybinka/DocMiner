using RAG.Repository;

namespace RAG.Requests
{
    public class DeleteEmbeddingsRequest
    {
        public IEmbeddingsRepository EmbeddingsRepository { get; set; }
        public ICollectionsRepository CollectionsRepository { get; set; }
        public string CollectionName { get; set; }
        public string[]? WhereDocumentNames { get; set; }
        public string[]? WhereChunkIds { get; set; }
    }
}
