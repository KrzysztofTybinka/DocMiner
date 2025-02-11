using Application.Abstractions;
using Application.Responses;
using ChromaDB.Client;
using Domain.Abstractions;


namespace Infrastructure.Queries.GetEmbeddingsById
{
    internal class GetEmbeddingsByIdQueryHandler : IGetEmbeddingsByIdQueryHandler
    {
        private readonly ChromaCollectionClient _chromaCollectionClient;

        public GetEmbeddingsByIdQueryHandler(ChromaCollectionClient client)
        {
            _chromaCollectionClient = client;
        }

        //No parameters == get all
        public async Task<Result<List<GetEmbeddingsByIdResponse>>> Handle(
            List<Guid>? ids,
            string? sourceDetails)
        {
            List<string>? idStrings = null;
            ChromaWhereOperator? whereClause = null;

            if (ids != null && ids.Count > 0)
            {
                idStrings = ids.Select(id => id.ToString()).ToList();
            }

            if (!string.IsNullOrEmpty(sourceDetails))
            {
                whereClause = ChromaWhereOperator.Equal("source", sourceDetails);
            }

            var result = await _chromaCollectionClient.Get(ids: idStrings,
                where: whereClause,
                include: ChromaGetInclude.Embeddings |
                ChromaGetInclude.Documents |
                ChromaGetInclude.Metadatas);

            //Not checking if result.count == 0 because 
            //empty collection is a success
            if (result == null)
            {
                return Result<List<GetEmbeddingsByIdResponse>>.Failure(GetEmbeddingsByIdQueryHandlerErrors.CollectionIsNull);
            }
            var mapperResult = result.FromChromaCollectionEntryToEmbeddings();

            if (!mapperResult.IsSuccess)
                return Result<List<GetEmbeddingsByIdResponse>>.Failure(mapperResult.Error);

            return Result<List<GetEmbeddingsByIdResponse>>.Success(mapperResult.Data);
        }
    }
}
