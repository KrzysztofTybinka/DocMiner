using ChromaDB.Client;
using Infrastructure.Repositories.ChromaCollection;
using Microsoft.AspNetCore.Mvc;
using RAG.Common;

namespace RAG.Endpoints
{
    public static class DocumentCollectionModule
    {
        public static void AddDocumentCollectionEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/DocumentCollection", async (string collectionName, ChromaCollectionRepository repository) =>
            {
                if (string.IsNullOrEmpty(collectionName))
                    return Results.BadRequest("Collection name cannot be empty.");

                await repository.CreateDocumentCollection(collectionName);
                return Results.Ok("Collection created");
            });

            app.MapDelete("/DocumentCollection", async (string collectionName, ChromaCollectionRepository repository) =>
            {
                if (string.IsNullOrEmpty(collectionName))
                    return Results.BadRequest("Collection name cannot be empty.");

                await repository.DeleteDocumentCollection(collectionName);
                return Results.Ok("Collection deleted");
            });

            app.MapGet("/DocumentCollections", async (ChromaCollectionRepository repository) =>
            {
                var result = await repository.ListDocumentCollections();
                return Results.Ok(result);
            });
        }
    }
}
