using Application.Abstractions;
using Application.Services;
using Domain.ProcessedDocument;
using Infrastructure.Services;
using RAG.Repository;
using RAG.Services;
using RAG.Services.Embedding;

namespace RAG.Requests
{
    public class CreateEmbeddingRequest
    {
        public IFormFile File {  get; set; }
        public string CollectionName { get; set; }
        public ProcessedDocumentService ProcessedDocumentService { get; set; }
        public IEmbeddingsRepository EmbeddingsRepository { get; set; }
        public ICollectionsRepository CollectionsRepository { get; set; }
        public IEmbeddingGeneratorFactory EmbeddingGeneratorFactory { get; set; }
        public int NumberOfTokens { get; set; } = 50;
    }
}
