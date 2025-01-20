
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RAG.BLL.Chunking;
using RAG.Common;
using RAG.Models;
using RAG.Repository;
using RAG.Services;
using RAG.Services.Embedding;
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
                    var result = await repository.GetDocumentCollections();

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

            app.MapPost("/embeddings", async (IFormFile file, OcrService ocrService, int numberOfTokens = 50) =>
            {
                if (file == null)
                    return Results.BadRequest("No file was uploaded.");

                if (numberOfTokens < 50)
                    return Results.BadRequest("Number of tokens can't be less than 50.");

                try
                {
                    //Ocr the file
                    var result = await ocrService.RequestOCRAsync(file);

                    //Clean up result
                    string cleanedResult = result.Data.Text.Replace("\r\n", " ").Replace("\n", " ");

                    //Split file into chunks
                    Chunker chunker = new(numberOfTokens, cleanedResult);
                    List<string> chunks = chunker.GetChunks();

                    //Get embedding service
                    EmbeddingServiceFactory embeddingServiceFactory = app.Services.GetRequiredService<EmbeddingServiceFactory>();
                    EmbeddingService embeddingModel = embeddingServiceFactory.CreateEmbeddingModel();

                    //Create embeddings
                    var embeddings = embeddingModel.CreateEmbeddingsAsync(chunks);

                    //Save embeddings into db //TO DO


                    if (result.IsSuccess)
                    {
                        return Results.Ok(result.Data);
                    }
                    else
                    {
                        return Results.BadRequest(result.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    return Results.StatusCode(500);
                }
            }).DisableAntiforgery();

            app.Run();
        }
    }
}
