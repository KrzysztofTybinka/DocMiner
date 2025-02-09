using Domain.Abstractions;

namespace Domain.Embedings
{
    public interface IEmbeddingGenerator
    {
        Task<Result<Embedding>> GenerateEmbeddingAsync(string text);
        Task<Result<List<Embedding>>> GenerateEmbeddingsAsync(List<string> textPieces);
    }
}
