using ChromaDB.Client.Models;
using RAG.Common;

namespace RAG.Repository
{
    public interface ICollectionsRepository
    {
        Task CreateDocumentCollection(string collectionName);
        Task DeleteDocumentCollection(string collectionName);
        Task<List<ChromaCollection>> ListDocumentCollections();
        Task<ChromaCollection> GetDocumentCollection(string collectionName);
    }
}
