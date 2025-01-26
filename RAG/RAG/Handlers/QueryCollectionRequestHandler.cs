using ChromaDB.Client;
using ChromaDB.Client.Models;
using RAG.BLL.Chunking;
using RAG.Common;
using RAG.Models;
using RAG.Requests;
using RAG.Services.Embedding;
using System.Collections;

namespace RAG.Handlers
{
    public static class QueryCollectionRequestHandler
    {
        public static async Task<Result<List<List<ChromaCollectionQueryEntry>>>> Handle(this QueryCollectionRequest request)
        {
            //Get embedding service
            EmbeddingService embeddingModel = request.EmbeddingServiceFactory.CreateEmbeddingModel();

            //Create embeddings
            var embeddingsResult = await embeddingModel.CreateEmbeddingsAsync(request.Prompts);

            if (!embeddingsResult.IsSuccess)
                return Result<List<List<ChromaCollectionQueryEntry>>>.Failure(embeddingsResult.ErrorMessage);

            //Get a collection
            var collection = await request.CollectionsRepository.GetDocumentCollection(request.CollectionName);

            //Query collection
            var queryResult = await request.EmbeddingsRepository.QueryCollection(collection,
                request.Nresults,
                embeddingsResult.Data);

            return Result<List<List<ChromaCollectionQueryEntry>>>.Success(queryResult);
        }
    }
}
