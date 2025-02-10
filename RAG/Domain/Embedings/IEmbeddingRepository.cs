
using Domain.Abstractions;

namespace Domain.Embedings
{
    public interface IEmbeddingRepository
    {
        Task<Result> UploadEmbeddingsAsync(List<Embedding> embeddings);

        Task<Result> DeleteEmbeddingsAsync(List<Guid> ids);

        Task<Result> UploadEmbeddingAsync(Embedding embedding);

        Task<Result> DeleteEmbeddingAsync(Guid id);
    }
}
