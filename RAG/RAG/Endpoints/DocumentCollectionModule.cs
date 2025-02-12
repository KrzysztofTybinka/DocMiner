using Infrastructure.Repositories.ChromaCollection;

namespace RAG.Endpoints
{
    public static class DocumentCollectionModule
    {
        public static void AddDocumentCollectionEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/document-collection", async (string collectionName, ChromaCollectionRepository repository) =>
            {
                if (string.IsNullOrEmpty(collectionName))
                    return Results.BadRequest("Collection name cannot be empty.");

                await repository.CreateDocumentCollection(collectionName);
                return Results.Ok("Collection created");
            });

            app.MapDelete("/document-collection", async (string collectionName, ChromaCollectionRepository repository) =>
            {
                if (string.IsNullOrEmpty(collectionName))
                    return Results.BadRequest("Collection name cannot be empty.");

                await repository.DeleteDocumentCollection(collectionName);
                return Results.Ok("Collection deleted");
            });

            app.MapGet("/document-collections", async (ChromaCollectionRepository repository) =>
            {
                var result = await repository.ListDocumentCollections();
                return Results.Ok(result);
            });
        }
    }
}
