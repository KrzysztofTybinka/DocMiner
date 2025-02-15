using Application.Responses;
using Domain.Abstractions;
using Infrastructure.Abstractions;
using MediatR;
using RAG.Requests;


namespace Application.Queries
{
    public class GetEmbeddingsByIdHandler : IRequestHandler<GetEmbeddingsQuery, Result<List<GetEmbeddingsByIdResponse>>>
    {
        private IGetEmbeddingsByIdQueryHandlerFactory _queryhandlerFactory { get; set; }
        public GetEmbeddingsByIdHandler(IGetEmbeddingsByIdQueryHandlerFactory queryhandlerFactor)
        {
            _queryhandlerFactory = queryhandlerFactor;
        }

        public async Task<Result<List<GetEmbeddingsByIdResponse>>> Handle(GetEmbeddingsQuery request, CancellationToken cancellationToken)
        {
            //Get propper collection
            var queryhandlerResult = await _queryhandlerFactory
                .CreateHandlerAsync(request.CollectionName);

            if (!queryhandlerResult.IsSuccess)
            {
                return Result<List<GetEmbeddingsByIdResponse>>.Failure(queryhandlerResult.Error);
            }

            var queryHandler = queryhandlerResult.Data;

            var ids = request.Ids == null || request.Ids.Length == 0 ? null
                : request.Ids
                .Select(id => new Guid(id))
                .ToList();

            //Query collection
            var result = await queryHandler.Handle(
                ids,
                request.Source);

            if (!result.IsSuccess)
            {
                return Result<List<GetEmbeddingsByIdResponse>>.Failure(result.Error);
            }

            return Result<List<GetEmbeddingsByIdResponse>>.Success(result.Data);
        }
    }
}
