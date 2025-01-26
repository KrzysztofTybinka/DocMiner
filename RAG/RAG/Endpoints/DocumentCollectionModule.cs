using ChromaDB.Client;
using Microsoft.AspNetCore.Mvc;
using RAG.Common;
using RAG.Repository;

namespace RAG.Endpoints
{
    public static class DocumentCollectionModule
    {
        public static void AddDocumentCollectionEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/DocumentCollection", async (string collectionName, ICollectionsRepository repository) =>
            {
                if (string.IsNullOrEmpty(collectionName))
                    return Results.BadRequest("Collection name cannot be empty.");

                try
                {
                    await repository.CreateDocumentCollection(collectionName);
                    return Results.Ok("Collection created");
                }
                catch (Exception ex)
                {
                    var problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "Server error",
                        Detail = ex.Message
                    };
                    return TypedResults.Problem(problemDetails);
                }
            });

            app.MapDelete("/DocumentCollection", async (string collectionName, ICollectionsRepository repository) =>
            {
                if (string.IsNullOrEmpty(collectionName))
                    return Results.BadRequest("Collection name cannot be empty.");

                try
                {
                    await repository.DeleteDocumentCollection(collectionName);
                    return Results.Ok("Collection deleted");
                }
                catch (Exception ex)
                {
                    var problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "Server error",
                        Detail = ex.Message
                    };
                    return TypedResults.Problem(problemDetails);
                }
            });

            app.MapGet("/DocumentCollections", async (ICollectionsRepository repository) =>
            {
                try
                {
                    var result = await repository.ListDocumentCollections();
                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    var problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "Server error",
                        Detail = ex.Message
                    };
                    return TypedResults.Problem(problemDetails);
                }
            });
        }
    }
}
