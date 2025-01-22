
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RAG.BLL.Chunking;
using RAG.Common;
using RAG.Endpoints;
using RAG.Handlers;
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

            app.AddDocumentCollectionEndpoints();
            app.AddEmbeddingsEndpoints();

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.Run();
        }
    }
}
