using RAG.Models;
using Microsoft.SemanticKernel.Connectors.Chroma;
using RAG.Common;
using Microsoft.Extensions.AI;
using System.Linq;
using RAG.BLL.Chunking;
using Microsoft.AspNetCore.Authentication;
using Microsoft.SemanticKernel.Memory;
using System.Text.Json.Serialization;

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

        public async Task<Result<List<ChromaCollectionModel>>> ListDocumentCollections()
        {
            var collectionNames = await _dbContext.ListCollectionsAsync()
                .ToListAsync();

            List<ChromaCollectionModel> collections = [];
            foreach (string name in collectionNames)
            {
                var collection = await GetDocumentCollection(name);
                collections.Add(collection);
            }
            return Result<List<ChromaCollectionModel>>.Success(collections);
        }

        public async Task<ChromaCollectionModel> GetDocumentCollection(string collectionName)
        {
            #pragma warning disable CS8603
            //Always not null here
            return await _dbContext.GetCollectionAsync(collectionName);
            #pragma warning restore CS8603
        }

        public IEnumerable<DocumentChunk> GetByDocumentName(int documentName, string collectionName)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<ChromaQueryResultModel>> QueryCollection(string collectionId, int nResults, float[][] embeddings)
        {
            ReadOnlyMemory<float>[] vectorEmbeddings = embeddings
                .Select(embedding => new ReadOnlyMemory<float>(embedding))
                .ToArray();

            var include = new string[] { "documents" };

            var result = await _dbContext.QueryEmbeddingsAsync(collectionId: collectionId, 
                queryEmbeddings: vectorEmbeddings, 
                nResults: nResults, 
                include: include);

            return Result<ChromaQueryResultModel>.Success(result);
        }

        public async Task<Result> UploadDocument(IEnumerable<DocumentChunk> chunks, string collectionId, string fileName)
        {
            if (string.IsNullOrWhiteSpace(collectionId))
                return Result.Failure("Collection name cannot be null or empty.");

            ReadOnlyMemory<float>[] vectorEmbeddings = chunks
                .Select(chunk => new ReadOnlyMemory<float>(chunk.EmbeddingVector.ToArray()))
                .ToArray();

            int index = 1;
            string[] ids = chunks
                .Select(chunk => fileName + "_" + index++)
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

    public class ChromaCollection
    {
        [JsonPropertyName("id")]
        public Guid Id { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("metadata")]
        public Dictionary<string, object>? Metadata { get; init; }

        [JsonPropertyName("tenant")]
        public string? Tenant { get; init; }

        [JsonPropertyName("database")]
        public string? Database { get; init; }

        public ChromaCollection(string name)
        {
            Name = name;
        }
    }
}
