using Application.Abstractions;
using Application.Responses;
using Domain.Abstractions;
using Infrastructure.Abstractions;
using MediatR;

namespace Application.Queries.GetSimilarEmbeddings
{
    public class GetSimilarEmbeddingsQuery : IRequest<Result<List<GetSimilarEmbeddingsResponse>>>
    {
        public string CollectionName { get; set; }
        public int Nresults { get; set; }
        public string Prompt { get; set; }
        public string? Source { get; set; }
        public double MaxDistance { get; set; }
        public IGetSimilarEmbeddingsQueryHandlerFactory QueryHandlerFactory { get; set; }
        public IEmbeddingGeneratorFactory EmbeddingGeneratorFactory { get; set; }
        public IEmbeddingRepositoryFactory EmbeddingsRepositoryFactory { get; set; }
    }
}
