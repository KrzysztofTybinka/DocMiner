using RAG.Common;

namespace RAG.Services.Embedding
{
    public interface IEmbedding
    {
        public Task<Result<List<Models.Embedding>>> CreateEmbeddingsAsync(List<string> chunks);
    }
}
