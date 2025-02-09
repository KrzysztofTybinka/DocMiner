using ChromaDB.Client;
using ChromaDB.Client.Models;
using RAG.Abstractions;
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
        public static async Task<IResult> Handle(this QueryCollectionRequest request)
        {
            //Get embedding service
            var embeddingModel = request.EmbeddingGeneratorFactory.CreateEmbeddingGenerator();

            //Create embeddings
            var embeddingResult = await embeddingModel.GenerateEmbeddingsAsync(request.Prompts);

            if (!embeddingResult.IsSuccess)
                return embeddingResult.ToProblemDetails();

            //Get a collection
            var collection = await request.CollectionsRepository.GetDocumentCollection(request.CollectionName);

            //Query collection
            var queryResult = await request.EmbeddingsRepository.QueryCollection(collection,
                request.Nresults,
                embeddingResult.Data);

            return Results.Ok(queryResult);
        }
    }
}
