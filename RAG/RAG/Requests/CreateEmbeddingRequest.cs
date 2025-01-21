using RAG.Repository;
using RAG.Services;

namespace RAG.Requests
{
    public class CreateEmbeddingRequest
    {
        public IFormFile File {  get; set; }
        public string DocumentCollectionName { get; set; }
        public OcrService OcrService { get; set; }
        public IEmbeddingsRepository Repository { get; set; }
        public int NumberOfTokens { get; set; } = 50;
    }
}
