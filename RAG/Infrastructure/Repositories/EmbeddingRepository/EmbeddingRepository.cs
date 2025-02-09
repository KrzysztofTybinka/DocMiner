
using ChromaDB.Client;
using ChromaDB.Client.Models;
using Domain.Abstractions;
using Domain.Embedings;


namespace Infrastructure.Repositories.DocumentRepository
{
    public class EmbeddingRepository : IEmbeddingRepository
    {
        private readonly ChromaCollectionClient _chromaCollectionClient;

        public EmbeddingRepository(IHttpClientFactory httpClientFactory,
            ChromaConfigurationOptions options,
            ChromaCollection collection)
        {
            var httpClient = httpClientFactory.CreateClient("ChromaDB");

            _chromaCollectionClient = new ChromaCollectionClient(
                collection,
                options,
                httpClient);
        }

        public async Task<Result> DeleteEmbeddingAsync(Guid id)
        {
            var idList = new List<Guid>() { id };
            return await DeleteEmbeddingsAsync(idList);
        }

        public async Task<Result> DeleteEmbeddingsAsync(List<Guid> ids)
        {
            List<string>? idStrings = null;

            if (ids == null || ids.Count == 0)
            {
                return Result.Failure(EmbeddingRepositoryErrors.DeleteEmbeddingIdsNull);
                
            }
            idStrings = ids.Select(id => id.ToString()).ToList();
            await _chromaCollectionClient.Delete(ids: idStrings);
            return Result.Success();
        }

        //No parameters == get all
        public async Task<Result<List<Embedding>>> GetEmbeddingsAsync(List<Guid>? ids, string? sourceDetails)
        {
            List<string>? idStrings = null;
            ChromaWhereOperator? whereClause = null;

            if (ids != null && ids.Count > 0)
            {
                idStrings = ids.Select(id => id.ToString()).ToList();
            }

            if (!string.IsNullOrEmpty(sourceDetails))
            {
                whereClause = ChromaWhereOperator.Equal("source", sourceDetails);
            }

            var result = await _chromaCollectionClient.Get(ids: idStrings, where: whereClause);

            //Not checking if result.count == 0 because 
            //empty collection is a success
            if (result == null)
            {
                return Result<List<Embedding>>.Failure(EmbeddingRepositoryErrors.CollectionIsNull);
            }
            var mapperResult = result.FromChromaCollectionEntryToEmbeddings();

            if (!mapperResult.IsSuccess)
                return Result<List<Embedding>>.Failure(mapperResult.Error);

            return Result<List<Embedding>>.Success(mapperResult.Data);
        }


        public async Task<Result<List<Embedding>>> QueryEmbeddingsAsync(Embedding embedding, 
            string sourceDetails, 
            int topResults = 10)
        {
            ChromaWhereOperator? whereClause = null;

            if (!string.IsNullOrEmpty(sourceDetails))
            {
                whereClause = ChromaWhereOperator.Equal("source", sourceDetails);
            }

            ReadOnlyMemory<float> queryEmbeddings = new ReadOnlyMemory<float>(
                embedding.TextEmbedding.ToArray());


            var result = await _chromaCollectionClient.Query(
                queryEmbeddings: queryEmbeddings,
                nResults: topResults,
                where: whereClause);

            var mapperResult = result.FromChromaCollectionQueryEntryToEmbeddings();
            if (!mapperResult.IsSuccess)
                return Result<List<Embedding>>.Failure(mapperResult.Error);

            return Result<List<Embedding>>.Success(mapperResult.Data);
        }

        public async Task<Result> UploadEmbeddingAsync(Embedding embedding)
        {
            var embeddings = new List<Embedding>() { embedding };
            return await UploadEmbeddingsAsync(embeddings);    
        }

        public async Task<Result> UploadEmbeddingsAsync(List<Embedding> embeddings)
        {
            var addRequest = embeddings.EmbeddingsToChromaDbAddRequest();

            await _chromaCollectionClient.Add(
                addRequest.Ids,
                addRequest.Vectors,
                addRequest.Metadatas,
                addRequest.TextContent);

            return Result.Success();
        }
    }
}
