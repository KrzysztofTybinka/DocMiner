using Microsoft.AspNetCore.Mvc;
using RAG.Repository;

namespace RAG.Endpoints
{
    public static class DocumentCollectionModule
    {
        public static void AddDocumentCollectionEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/DocumentCollection", async (string collectionName, IEmbeddingsRepository repository) =>
            {
                try
                {
                    var result = await repository.CreateDocumentCollection(collectionName);

                    if (result.IsSuccess)
                    {
                        return Results.Ok("Collection created");
                    }
                    else
                    {
                        return Results.BadRequest(result.ErrorMessage);
                    }
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

            app.MapDelete("/DocumentCollection", async (string collectionName, IEmbeddingsRepository repository) =>
            {
                try
                {
                    var result = await repository.DeleteDocumentCollection(collectionName);

                    if (result.IsSuccess)
                    {
                        return Results.Ok("Collection deleted");
                    }
                    else
                    {
                        return Results.BadRequest(result.ErrorMessage);
                    }
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

            app.MapGet("/DocumentCollections", async (IEmbeddingsRepository repository) =>
            {
                try
                {
                    var result = await repository.ListDocumentCollections();

                    if (result.IsSuccess)
                    {
                        return Results.Ok(result);
                    }
                    else
                    {
                        return Results.BadRequest(result.ErrorMessage);
                    }
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
