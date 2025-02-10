
using ChromaDB.Client;
using Domain.Abstractions;
using Infrastructure.Repositories.Collection;

namespace Infrastructure.Repositories.ChromaCollection
{
    public class ChromaCollectionRepository
    {
        private readonly ChromaClient _client;

        public ChromaCollectionRepository(ChromaClient client)
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

        public async Task<List<ChromaDB.Client.Models.ChromaCollection>> ListDocumentCollections()
        {
            return await _client.ListCollections();
        }

        public async Task<ChromaDB.Client.Models.ChromaCollection> GetDocumentCollection(string collectionName)
        {
            return await _client.GetCollection(collectionName);
        }
    }
}
