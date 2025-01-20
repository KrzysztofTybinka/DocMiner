using RAG.Common;
using RAG.Models;

namespace RAG.Repository
{
    public interface IEmbeddingsRepository
    {
        IEnumerable<Embedding> GetByDocumentId(int id);
        IEnumerable<Embedding> GetSimilar(Embedding embedding,int similarityTreshold);
        void Add(Embedding embedding);
        void AddRange(IEnumerable<Embedding> embeddings);
        void Delete(int embeddingId);
        void DeleteDocument(int documentId);
        Task<Result> CreateDocumentCollection(string collectionName);
        Task<Result> DeleteDocumentCollection(string collectionName);
    }
}
