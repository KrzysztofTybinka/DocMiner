using Application.Abstractions;
using ChromaDB.Client.Models;
using Domain.Embedings;
using Infrastructure.Abstractions;
using Infrastructure.Repositories.ChromaCollection;

namespace RAG.Requests
{
    public class QueryCollectionRequest
    {
        public string CollectionName { get; set; }
        public int Nresults { get; set; }
        public string Prompt { get; set; }
        public string? Source { get; set; }
        public double MinDistance { get; set; }
        public IGetSimilarEmbeddingsQueryHandlerFactory QueryHandlerFactory { get; set; }
        public IEmbeddingGeneratorFactory EmbeddingGeneratorFactory { get; set; }
        public IEmbeddingRepositoryFactory EmbeddingsRepositoryFactory { get; set; }
    }
}
