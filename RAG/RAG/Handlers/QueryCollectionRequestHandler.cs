using Microsoft.SemanticKernel.Connectors.Chroma;
using RAG.BLL.Chunking;
using RAG.Common;
using RAG.Requests;
using RAG.Services.Embedding;

namespace RAG.Handlers
{
    public static class QueryCollectionRequestHandler
    {
        #pragma warning disable SKEXP0020
        public static async Task<Result<ChromaQueryResultModel>> Handle(this QueryCollectionRequest request)
        {
            //Get embedding service
            EmbeddingService embeddingModel = request.EmbeddingServiceFactory.CreateEmbeddingModel();

            //Create embeddings
            var embeddingsResult = await embeddingModel.CreateEmbeddingsAsync(request.Prompts);

            if (!embeddingsResult.IsSuccess)
                return Result<ChromaQueryResultModel>.Failure(embeddingsResult.ErrorMessage);

            var embeddings = embeddingsResult.Data
                .Select(embedding => 
                    embedding.EmbeddingVector.ToArray())
                .ToArray();

            //Query collection
            var queryResult = await request.Repository.QueryCollection(request.CollectionId,
                request.Nresults,
                embeddings);

            if (!queryResult.IsSuccess)
                return Result<ChromaQueryResultModel>.Failure(queryResult.ErrorMessage);

            return Result<ChromaQueryResultModel>.Success(queryResult.Data);
        }
        #pragma warning restore SKEXP0020
    }
}
