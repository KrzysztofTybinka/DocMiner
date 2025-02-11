
using Application.Abstractions;
using Application.Responses;
using ChromaDB.Client;
using Domain.Abstractions;
using Domain.Embedings;
using Infrastructure.Repositories.DocumentRepository;

namespace Infrastructure.Queries.GetSimilarEmbeddings
{
    public class GetSimilarEmbeddingsQueryHandler : IGetSimilarEmbeddingsQueryHandler
    {
        private readonly ChromaCollectionClient _chromaCollectionClient;

        public GetSimilarEmbeddingsQueryHandler(ChromaCollectionClient client)
        {
            _chromaCollectionClient = client;
        }

        public async Task<Result<List<GetSimilarEmbeddingsResponse>>> Handle(
            Embedding embedding,
            string? sourceDetails,
            double minDistance,
            int topResults = 10)
        {
            ChromaWhereOperator? whereClause = null;

            if (!string.IsNullOrEmpty(sourceDetails))
            {
                whereClause = ChromaWhereOperator.Equal("source", sourceDetails);
            }

            ReadOnlyMemory<float> queryEmbeddings = new ReadOnlyMemory<float>(
                embedding.TextEmbedding.ToArray());

            var result = await _chromaCollectionClient.Query(
                queryEmbeddings: queryEmbeddings,
                nResults: topResults,
                where: whereClause,
                include: ChromaQueryInclude.Metadatas |
                ChromaQueryInclude.Distances |
                ChromaQueryInclude.Documents);

            var mapperResult = result.FromChromaCollectionQueryEntryToSimilarEmbeddingsResponse();

            mapperResult = mapperResult
                .Where(x => x.Distance >= minDistance)
                .ToList();

            return Result<List<GetSimilarEmbeddingsResponse>>.Success(mapperResult);
        }
    }
}
