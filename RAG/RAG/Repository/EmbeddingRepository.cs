using RAG.Models;
using Microsoft.SemanticKernel.Connectors.Chroma;

namespace RAG.Repository
{
    public class EmbeddingRepository : IEmbeddingsRepository
    {
        #pragma warning disable SKEXP0020

        private readonly HttpClient _dbContext;

        public EmbeddingRepository(IHttpClientFactory httpClientFactory)
        {
            _dbContext = httpClientFactory.CreateClient("chromadb");
        }


        public async Task CreateDocumentCollection()
        {
            var c = new ChromaClient("http://host.docker.internal:8000");
            await c.CreateCollectionAsync("test2");
            var xd = await c.ListCollectionsAsync().ToListAsync();
        }


        public void Add(Embedding embedding)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<Embedding> embeddings)
        {
            throw new NotImplementedException();
        }

        public void Delete(int embeddingId)
        {
            throw new NotImplementedException();
        }

        public void DeleteDocument(int documentId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Embedding> GetByDocumentId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Embedding> GetSimilar(Embedding embedding, int similarityTreshold)
        {
            throw new NotImplementedException();
        }

        #pragma warning restore SKEXP0020
    }
}
