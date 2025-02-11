
using Application.Queries;
using Application.Responses;
using Domain.Abstractions;
using Infrastructure.Abstractions;
using MediatR;

namespace RAG.Requests
{
    public class GetEmbeddingsQuery : IRequest<Result<List<GetEmbeddingsByIdResponse>>>
    {
        public IGetEmbeddingsByIdQueryHandlerFactory QueryhandlerFactory { get; set; }
        public string CollectionName { get; set; }
        public string[]? Ids {  get; set; }
        public string? Source {  get; set; }
    }
}
