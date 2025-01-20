using RAG.Models;
using Microsoft.SemanticKernel.Connectors.Chroma;
using RAG.Common;

namespace RAG.Repository
{
    public class EmbeddingsRepository : IEmbeddingsRepository
    {
        #pragma warning disable SKEXP0020

        private readonly ChromaClient _dbContext;

        public EmbeddingsRepository(string chromaUrl)
        {
            _dbContext = new ChromaClient(chromaUrl);
        }


        public async Task<Result> CreateDocumentCollection(string collectionName)
        {
            if (string.IsNullOrWhiteSpace(collectionName))
                return Result.Failure("Collection name cannot be null or empty.");

            await _dbContext.CreateCollectionAsync(collectionName);
            return Result.Success();
        }

        public async Task<Result> DeleteDocumentCollection(string collectionName)
        {
            if (string.IsNullOrWhiteSpace(collectionName))
                return Result.Failure("Collection name cannot be null or empty.");

            await _dbContext.DeleteCollectionAsync(collectionName);
            return Result.Success();
        }

        public async Task<Result<List<string>>> GetDocumentCollections()
        {
            var collections = await _dbContext.ListCollectionsAsync()
                .ToListAsync();

            return Result<List<string>>.Success(collections);
        }

        public async Task<Result<ChromaCollectionModel>> GetDocumentCollection(string collectionName)
        {
            var collections = await _dbContext.GetCollectionAsync(collectionName);

            if (collections == null)
                return Result<ChromaCollectionModel>.Failure($"Collection {collectionName} doesn't exist.");

            return Result<ChromaCollectionModel>.Success(collections);
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
