
using RAG.Handlers;
using RAG.Requests;
using RAG.Validators;

namespace RAG.Endpoints
{
    public static class EmbeddingsModule
    {
        public static void AddEmbeddingsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/Embeddings", async ([AsParameters] CreateEmbeddingRequest request) =>
            {
                var validationResult = request.IsValid();

                if (!validationResult.IsValid)
                    return Results.BadRequest(validationResult.Errors);

                return await request.Handle();
            }).DisableAntiforgery();

            app.MapPost("/QueryEmbeddings", async ([AsParameters] QueryCollectionRequest request) =>
            {
                var validationResult = request.IsValid();

                if (!validationResult.IsValid)
                    return Results.BadRequest(validationResult.Errors);

                return await request.Handle();
            });

            app.MapGet("/Embeddings", async ([AsParameters] GetCollectionRequest request) =>
            {
                if (String.IsNullOrEmpty(request.CollectionName))
                    return Results.BadRequest("Collection name cannot be empty.");

                return await request.Handle();
            });

            app.MapDelete("/Embeddings", async ([AsParameters] DeleteEmbeddingsRequest request) =>
            {
                if (String.IsNullOrEmpty(request.CollectionName))
                    return Results.BadRequest("Collection name cannot be empty.");

                return await request.Handle();
            });
        }
        
    }
}
