using RAG.Models;
using Microsoft.SemanticKernel.Connectors.Chroma;
using RAG.Common;
using Microsoft.Extensions.AI;
using System.Linq;

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

        public async Task<Result<List<string>>> ListDocumentCollections()
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

        public IEnumerable<DocumentChunk> GetByDocumentName(int documentName, string collectionName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DocumentChunk> QueryCollection(DocumentChunk embedding, int similarityTreshold)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UploadDocument(IEnumerable<DocumentChunk> chunks, string collectionId)
        {
            if (string.IsNullOrWhiteSpace(collectionId))
                return Result.Failure("Collection name cannot be null or empty.");

            ReadOnlyMemory<float>[] vectorEmbeddings = chunks
                .Select(chunk => new ReadOnlyMemory<float>(chunk.EmbeddingVector.ToArray()))
                .ToArray();

            string[] ids = chunks
                .Select(chunk => chunk.Id)
                .ToArray();

            await _dbContext.UpsertEmbeddingsAsync(collectionId, ids, vectorEmbeddings);

            return Result.Success();
        }

        public void UploadManyDocuments(IEnumerable<DocumentChunk> embeddings, string collectionName)
        {
            throw new NotImplementedException();
        }

        public void DeleteDocument(int documentName, string collectionName)
        {
            throw new NotImplementedException();
        }




#pragma warning restore SKEXP0020
    }
}
