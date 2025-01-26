using ChromaDB.Client;
using ChromaDB.Client.Models;
using RAG.Common;
using RAG.Models;

namespace RAG.Repository
{
    public interface IEmbeddingsRepository
    {
        //Documents CRUD, documents are uploaded into the collections (one collection has many documents)
        Task<List<ChromaCollectionEntry>> GetCollection(ChromaCollection collection,
            List<string>? ids = null,
            ChromaWhereOperator? where = null,
            ChromaWhereDocumentOperator? whereDocument = null,
            int? limit = null,
            int? offset = null,
            ChromaGetInclude? include = null);

        Task<List<List<ChromaCollectionQueryEntry>>> QueryCollection(
            ChromaCollection collection, int nResults,
            IEnumerable<DocumentChunk> embeddings, 
            ChromaWhereOperator? where = null,
            ChromaWhereDocumentOperator? whereDocument = null,
            ChromaQueryInclude? include = null);

        Task UploadDocument(IEnumerable<DocumentChunk> chunks, ChromaCollection collection);
        Task DeleteDocument(int documentName, ChromaCollection collection);
    }
}
