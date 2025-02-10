
using Application.Queries;
using Infrastructure.Abstractions;

namespace RAG.Requests
{
    public class GetCollectionRequest
    {
        public IGetEmbeddingsByIdQueryHandlerFactory QueryhandlerFactory { get; set; }
        public string CollectionName { get; set; }
        public string[]? Ids {  get; set; }
        public string? Source {  get; set; }
    }
}
