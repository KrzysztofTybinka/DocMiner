
using Application.Responses;
using Domain.Abstractions;
using Domain.Embedings;

namespace Application.Queries
{
    public interface IGetSimilarEmbeddingsQueryHandler
    {
        Task<Result<List<GetSimilarEmbeddingsResponse>>> Handle(Embedding embedding,
            string? sourceDetails,
            double maxDistance,
            int topResults = 10);
    }
}
