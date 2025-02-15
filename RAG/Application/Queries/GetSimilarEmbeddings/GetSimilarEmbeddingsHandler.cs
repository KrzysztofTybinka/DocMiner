using Application.Abstractions;
using Application.Responses;
using Domain.Abstractions;
using Infrastructure.Abstractions;
using MediatR;


namespace Application.Queries.GetSimilarEmbeddings
{
    public class GetSimilarEmbeddingsHandler : IRequestHandler<GetSimilarEmbeddingsQuery, Result<List<GetSimilarEmbeddingsResponse>>>
    {
        private IGetSimilarEmbeddingsQueryHandlerFactory _queryHandlerFactory { get; set; }
        private IEmbeddingGeneratorFactory _embeddingGeneratorFactory { get; set; }

        public GetSimilarEmbeddingsHandler(
            IGetSimilarEmbeddingsQueryHandlerFactory queryHandlerFactory,
            IEmbeddingGeneratorFactory embeddingGeneratorFactory)
        {
            _queryHandlerFactory = queryHandlerFactory;
            _embeddingGeneratorFactory = embeddingGeneratorFactory;
        }

        public async Task<Result<List<GetSimilarEmbeddingsResponse>>> Handle(GetSimilarEmbeddingsQuery request, CancellationToken cancellationToken)
        {
            //Get propper collection
            var queryHandlerResult = await _queryHandlerFactory
                .CreateHandlerAsync(request.CollectionName);

            if (!queryHandlerResult.IsSuccess)
            {
                return Result<List<GetSimilarEmbeddingsResponse>>
                    .Failure(queryHandlerResult.Error);
            }

            var queryHandler = queryHandlerResult.Data;

            //Get embedding service
            var embeddingModel = _embeddingGeneratorFactory
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
                request.MaxDistance,
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
