using Microsoft.Extensions.Options;
using RAG.Common;
using RAG.Models;

namespace RAG.Services.Embedding
{
    public abstract class EmbeddingService
    {
        protected readonly EmbeddingModelSettings _embeddingModelSettings;

        protected EmbeddingService(IOptions<EmbeddingModelSettings> embeddingModelSettings)
        {
            _embeddingModelSettings = embeddingModelSettings.Value;
        }

        public abstract Task<Result<List<Models.Embedding>>> CreateEmbeddingsAsync(List<string> chunks);
    }
}
