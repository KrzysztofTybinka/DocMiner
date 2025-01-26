using RAG.Repository;
using RAG.Services;
using RAG.Services.Embedding;

namespace RAG.Requests
{
    public class CreateEmbeddingRequest
    {
        public IFormFile File {  get; set; }
        public string CollectionName { get; set; }
        public OcrService OcrService { get; set; }
        public IEmbeddingsRepository EmbeddingsRepository { get; set; }
        public ICollectionsRepository CollectionsRepository { get; set; }
        public EmbeddingServiceFactory EmbeddingServiceFactory { get; set; }
        public int NumberOfTokens { get; set; } = 50;
    }
}
