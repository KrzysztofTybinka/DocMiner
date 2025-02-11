using Application.Responses;
using Domain.Abstractions;
using MediatR;


namespace Application.Queries.GetSimilarEmbeddings
{
    public class GetSimilarEmbeddingsHandler : IRequestHandler<GetSimilarEmbeddingsQuery, Result<List<GetSimilarEmbeddingsResponse>>>
    {
        public async Task<Result<List<GetSimilarEmbeddingsResponse>>> Handle(GetSimilarEmbeddingsQuery request, CancellationToken cancellationToken)
        {
            //Get propper collection
            var queryHandlerResult = await request
                .QueryHandlerFactory
                .CreateHandlerAsync(request.CollectionName);

            if (!queryHandlerResult.IsSuccess)
            {
                return Result<List<GetSimilarEmbeddingsResponse>>
                    .Failure(queryHandlerResult.Error);
            }

            var queryHandler = queryHandlerResult.Data;

            //Get embedding service
            var embeddingModel = request.EmbeddingGeneratorFactory
                .CreateEmbeddingGenerator();

            //Create embeddings
            var embeddingResult = await embeddingModel
                .GenerateEmbeddingAsync(request.Prompt);

            if (!embeddingResult.IsSuccess)
            {
                return Result<List<GetSimilarEmbeddingsResponse>>
                   .Failure(embeddingResult.Error);
            }

            //Query collection
            var queryResult = await queryHandler
                .Handle(embeddingResult.Data,
                request.Source,
                request.MinDistance,
                request.Nresults);

            if (!queryResult.IsSuccess)
            {
                return Result<List<GetSimilarEmbeddingsResponse>>
                    .Failure(queryResult.Error);
            }

            return Result<List<GetSimilarEmbeddingsResponse>>
                .Success(queryResult.Data);
        }
    }
}
