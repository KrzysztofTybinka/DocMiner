
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RAG.BLL.Chunking;
using RAG.Common;
using RAG.Models;
using RAG.Repository;
using RAG.Requests;
using RAG.Services;
using RAG.Services.Embedding;
using RAG.Validators;
using System.Collections;
using System.Net.Http.Headers;

namespace RAG
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<OcrService>();

            var tesseractUrl = builder.Configuration["OCR_URL"] ?? "http://host.docker.internal:8081";

            builder.Services.AddHttpClient("ocr", client =>
            {
                client.BaseAddress = new Uri(tesseractUrl);
            });

            var chromaApiUrl = builder.Configuration.GetValue<string>("ChromaDbUrl") ?? "http://host.docker.internal:8000";

            // Register the repository with the Chroma API URL
            builder.Services.AddSingleton<IEmbeddingsRepository>(provider =>
                new EmbeddingsRepository(chromaApiUrl));

            builder.Services.Configure<EmbeddingModelSettings>(
                builder.Configuration.GetSection("EmbeddingModelSettings"));

            builder.Services.AddSingleton<EmbeddingServiceFactory>();
            builder.Services.AddSingleton<EmbeddingService, OpenAIEmbeddingService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

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

            app.MapGet("/DocumentCollection", async (string collectionName, IEmbeddingsRepository repository) =>
            {
                try
                {
                    var result = await repository.GetDocumentCollection(collectionName);

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

            app.MapPost("/embeddings", async (CreateEmbeddingRequest request) =>
            {
                var validationResult = request.IsValid();

                if (!validationResult.IsValid)
                    return Results.BadRequest(validationResult.Errors);

                try
                {
                    //Ocr the file
                    var result = await request.OcrService.RequestOCRAsync(request.File);

                    if(!result.IsSuccess)
                        return Results.BadRequest(result.ErrorMessage);

                    //Clean up result
                    string cleanedResult = result.Data.Text.Replace("\r\n", " ").Replace("\n", " ");

                    //Split file into chunks
                    Chunker chunker = new(request.NumberOfTokens, cleanedResult);
                    List<string> chunks = chunker.GetChunks();

                    //Get embedding service
                    EmbeddingServiceFactory embeddingServiceFactory = app.Services.GetRequiredService<EmbeddingServiceFactory>();
                    EmbeddingService embeddingModel = embeddingServiceFactory.CreateEmbeddingModel();

                    //Create embeddings
                    var embeddingsResult = await embeddingModel.CreateEmbeddingsAsync(chunks, request.File.Name);

                    if (!embeddingsResult.IsSuccess)
                        return Results.BadRequest(result.ErrorMessage);

                    //Save embeddings into db //TO DO
                    var uploadResult = await request.Repository.UploadDocument(embeddingsResult.Data, request.File.Name);

                    return Results.Ok(result.Data);
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

            app.Run();
        }
    }
}
