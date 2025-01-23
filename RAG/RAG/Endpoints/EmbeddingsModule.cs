﻿using Microsoft.AspNetCore.Mvc;
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

                try
                {
                    var result = await request.Handle();

                    if (result.IsSuccess)
                    {
                        return Results.Ok("Embeddings created.");
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
            }).DisableAntiforgery();

            app.MapGet("Embeddings", async ([AsParameters] QueryCollectionRequest request) =>
            {
                var validationResult = request.IsValid();

                if (!validationResult.IsValid)
                    return Results.BadRequest(validationResult.Errors);

                //try
                //{
                    var result = await request.Handle();

                    if (result.IsSuccess)
                    {
                        return Results.Ok(result);
                    }
                    else
                    {
                        return Results.BadRequest(result.ErrorMessage);
                    }
                //}
                //catch (Exception ex)
                //{
                //    var problemDetails = new ProblemDetails
                //    {
                //        Status = StatusCodes.Status500InternalServerError,
                //        Title = "Server error",
                //        Detail = ex.Message
                //    };
                //    return TypedResults.Problem(problemDetails);
                //}
            });
        }
    }
}
