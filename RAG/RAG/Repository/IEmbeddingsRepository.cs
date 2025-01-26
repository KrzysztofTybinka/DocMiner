using ChromaDB.Client;
using ChromaDB.Client.Models;
using RAG.Common;
using RAG.Models;

namespace RAG.Repository
{
    public interface IEmbeddingsRepository
    {
        //Documents CRUD, documents are uploaded into the collections (one collection has many documents)
        //IEnumerable<DocumentChunk> GetByDocumentName(int documentName, string collectionName); to do?

        Task<List<List<ChromaCollectionQueryEntry>>> QueryCollection(
            ChromaCollection collection, int nResults,
            IEnumerable<DocumentChunk> embeddings, 
            ChromaWhereOperator? where = null,
            ChromaWhereDocumentOperator? whereDocument = null,
            ChromaQueryInclude? include = null);

        Task UploadDocument(IEnumerable<DocumentChunk> chunks, ChromaCollection collection, string fileName);
        //void DeleteDocument(int documentName, string collectionName); to do
    }
}
