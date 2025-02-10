using ChromaDB.Client.Models;
using Domain.Embedings;
using RAG.Abstractions;
using RAG.Requests;

namespace RAG.Handlers
{
    public static class QueryCollectionRequestHandler
    {
        public static async Task<IResult> Handle(this QueryCollectionRequest request)
        {
            //Get propper collection
            var queryHandlerResult = await request
                .QueryHandlerFactory
                .CreateHandlerAsync(request.CollectionName);

            if (!queryHandlerResult.IsSuccess)
            {
                return queryHandlerResult.ToProblemDetails();
            }

            var queryHandler = queryHandlerResult.Data;

            //Get embedding service
            var embeddingModel = request.EmbeddingGeneratorFactory
                .CreateEmbeddingGenerator();

            //Create embeddings
            var embeddingResult = await embeddingModel
                .GenerateEmbeddingAsync(request.Prompt);

            if (!embeddingResult.IsSuccess)
                return embeddingResult.ToProblemDetails();

            //Query collection
            var queryResult = await queryHandler
                .Handle(embeddingResult.Data,
                request.Source,
                request.MinDistance,
                request.Nresults);

            if (!queryResult.IsSuccess)
            {
                return queryResult.ToProblemDetails();
            }

            return Results.Ok(queryResult.Data);
        }
    }
}
