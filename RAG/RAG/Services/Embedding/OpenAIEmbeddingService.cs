using Microsoft.Extensions.Options;
using RAG.Common;
using RAG.Models;

namespace RAG.Services.Embedding
{
    public class OpenAIEmbeddingService : EmbeddingService
    {
        public OpenAIEmbeddingService(IOptions<EmbeddingModelSettings> embeddingModelSettings) : base(embeddingModelSettings) { }

        public override async Task<Result<List<Models.Embedding>>> CreateEmbeddingsAsync(List<string> chunks)
        {
            throw new NotImplementedException();
        }
    }
}
