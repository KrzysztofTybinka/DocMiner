
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using RAG.BLL.Chunking;
using RAG.Common;
using RAG.Models;
using RAG.Services;
using RAG.Services.Embedding;
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

            builder.Services.Configure<EmbeddingModelSettings>(builder.Configuration.GetSection("EmbeddingModelSettings"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();


            app.MapPost("/ocr", async (IFormFile file, OcrService ocrService) =>
            {
                if (file == null)
                    return Results.BadRequest("No file was uploaded.");

                try
                {
                    var result = await ocrService.RequestOCRAsync(file);

                    var chunker = new Chunker(200, result.Data.Text);
                    chunker.GetChunks();

                    if (result.IsSuccess)
                    {
                        return Results.Ok(result.Data);
                    }
                    else
                    {
                        return Results.BadRequest(result.ErrorMessage);
                    }
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            }).DisableAntiforgery();

            app.MapPost("/embeddings", async (IFormFile file, OcrService ocrService) =>
            {
                if (file == null)
                    return Results.BadRequest("No file was uploaded.");

                try
                {
                    //Ocr the file
                    var result = await ocrService.RequestOCRAsync(file);

                    //Clean up result? TO DO?

                    //Split file into chunks
                    Chunker chunker = new(200, result.Data.Text);
                    List<string> chunks = chunker.GetChunks();

                    //Get embedding service
                    var embeddingModelSettings = app.Services.GetRequiredService<EmbeddingModelSettings>();
                    var embeddingServiceFactory = new EmbeddingFactory(embeddingModelSettings);
                    IEmbedding embeddingModel = embeddingServiceFactory.CreateEmbeddingModel();

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
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            }).DisableAntiforgery();

            app.Run();
        }
    }
}
