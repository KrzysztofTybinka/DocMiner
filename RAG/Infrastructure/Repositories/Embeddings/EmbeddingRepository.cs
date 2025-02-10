
using ChromaDB.Client;
using ChromaDB.Client.Models;
using Domain.Abstractions;
using Domain.Embedings;


namespace Infrastructure.Repositories.DocumentRepository
{
    public class EmbeddingRepository : IEmbeddingRepository
    {
        private readonly ChromaCollectionClient _chromaCollectionClient;

        public EmbeddingRepository(ChromaCollectionClient client)
        {
            _chromaCollectionClient = client;
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
