using ChromaDB.Client;
using ChromaDB.Client.Models;
using RAG.Common;
using RAG.Requests;

namespace RAG.Handlers
{
    public static class GetCollectionRequestHandler
    {
        public static async Task<Result<List<ChromaCollectionEntry>>> Handle(this GetCollectionRequest request)
        {
            var collection = await request.CollectionsRepository.GetDocumentCollection(request.CollectionName);
            var ids = request.WhereChunkIds == null || request.WhereChunkIds.Length == 0 ? null : request.WhereChunkIds.ToList();
            var metadata = request.WhereDocumentNames == null || request.WhereDocumentNames.Length == 0 ?
            null :
            ChromaWhereOperator.Equal("file name", request.WhereDocumentNames);

            var result = await request.EmbeddingsRepository.GetCollection(
                collection,
                ids,
                metadata);

            return Result<List<ChromaCollectionEntry>>.Success(result);
        }
    }
}
