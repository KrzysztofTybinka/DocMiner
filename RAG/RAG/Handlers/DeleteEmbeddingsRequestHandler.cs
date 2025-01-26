using ChromaDB.Client;
using ChromaDB.Client.Models;
using RAG.Common;
using RAG.Requests;

namespace RAG.Handlers
{
    public static class DeleteEmbeddingsRequestHandler
    {
        public static async Task<Result> Handle(this DeleteEmbeddingsRequest request)
        {
            var collection = await request.CollectionsRepository.GetDocumentCollection(request.CollectionName);
            var ids = request.WhereChunkIds == null || request.WhereChunkIds.Length == 0 ? null : request.WhereChunkIds.ToList();
            var metadata = request.WhereDocumentNames == null || request.WhereDocumentNames.Length == 0 ?
            null :
            ChromaWhereOperator.Equal("file name", request.WhereDocumentNames);

            await request.EmbeddingsRepository.DeleteEmbeddings(
                collection,
                ids,
                metadata);

            return Result.Success();
        }
    }
}
