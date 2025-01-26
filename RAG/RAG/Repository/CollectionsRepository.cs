using RAG.Models;
using RAG.Common;
using System.Linq;
using RAG.BLL.Chunking;
using Microsoft.AspNetCore.Authentication;
using System.Text.Json.Serialization;
using ChromaDB.Client;
using ChromaDB.Client.Models;

namespace RAG.Repository
{
    public class CollectionsRepository : ICollectionsRepository
    {
        private readonly ChromaClient _client;

        public CollectionsRepository(ChromaClient client)
        {
            _client = client;
        }

        public async Task CreateDocumentCollection(string collectionName)
        {
            await _client.CreateCollection(collectionName);
        }

        public async Task DeleteDocumentCollection(string collectionName)
        {
            await _client.DeleteCollection(collectionName);
        }

        public async Task<List<ChromaCollection>> ListDocumentCollections()
        {
            return await _client.ListCollections();
        }

        public async Task<ChromaCollection> GetDocumentCollection(string collectionName)
        {
            return await _client.GetCollection(collectionName);
        }
    }
}
