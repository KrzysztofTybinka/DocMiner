using Microsoft.SemanticKernel.Connectors.Chroma;
using RAG.Common;
using RAG.Models;

namespace RAG.Repository
{
    public interface IEmbeddingsRepository
    {
        //Documents CRUD, documents are uploaded into the collections (one collection has many documents)
        IEnumerable<DocumentChunk> GetByDocumentName(int documentName, string collectionName);
        IEnumerable<DocumentChunk> QueryCollection(DocumentChunk embedding,int similarityTreshold);
        Task<Result> UploadDocument(IEnumerable<DocumentChunk> embeddings, string collectionName);
        void UploadManyDocuments(IEnumerable<DocumentChunk> embeddings, string collectionName);
        void DeleteDocument(int documentName, string collectionName);

        //Collections CRUD
        Task<Result> CreateDocumentCollection(string collectionName);
        Task<Result> DeleteDocumentCollection(string collectionName);
        Task<Result<List<string>>> ListDocumentCollections();
        #pragma warning disable SKEXP0020
        Task<Result<ChromaCollectionModel>> GetDocumentCollection(string collectionName);
        #pragma warning restore SKEXP0020
    }
}
