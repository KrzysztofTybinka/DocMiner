using RAG.Common;
using RAG.Models;

namespace RAG.Services.Embedding
{
    public class OpenAIEmbeddingService : IEmbedding
    {
        private readonly EmbeddingModelSettings _embeddingModelSettings;

        public OpenAIEmbeddingService(EmbeddingModelSettings embeddingModelSettings)
        {
            _embeddingModelSettings = embeddingModelSettings;
        }

        Task<Result<List<Models.Embedding>>> IEmbedding.CreateEmbeddingsAsync(List<string> chunks)
        {
            throw new NotImplementedException();
        }
    }
}
