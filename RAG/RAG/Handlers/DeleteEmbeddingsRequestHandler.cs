using ChromaDB.Client;
using ChromaDB.Client.Models;
using Domain.Abstractions;
using RAG.Abstractions;
using RAG.Requests;

namespace RAG.Handlers
{
    public static class DeleteEmbeddingsRequestHandler
    {
        public static async Task<IResult> Handle(this DeleteEmbeddingsRequest request)
        {
            if (request.Ids == null || request.Ids.Length == 0)
                return Results.BadRequest("You need to pass at leastone embedding Id");

            //Get propper collection
            var embeddingsRepositoryResult = await request
                .EmbeddingsRepositoryFactory
                .CreateRepositoryAsync(request.CollectionName);

            if (!embeddingsRepositoryResult.IsSuccess)
            {
                return embeddingsRepositoryResult.ToProblemDetails();
            }

            var ids = request.Ids
                .Select(id => new Guid(id))
                .ToList();

            var embeddingsRepository = embeddingsRepositoryResult.Data;
            await embeddingsRepository.DeleteEmbeddingsAsync(ids);

            return Results.Ok("Embeddings deleted.");
        }
    }
}
