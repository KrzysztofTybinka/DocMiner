
using Application.Services;
using ChromaDB.Client;
using Domain.ProcessedDocument;
using Infrastructure.Services;
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
            var enableSwagger = builder.Configuration.GetValue<bool>("SwaggerSettings:EnableSwagger");

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Register OCR service =========================================================================
            var tesseractUrl = builder.Configuration["ExternalServices:OcrUrl"] ??
                throw new ArgumentNullException("Ocr url address was empty.");

            builder.Services.AddHttpClient<IProcessedDocumentGenerator, OcrService>(client =>
            {
                client.BaseAddress = new Uri(tesseractUrl);
            });
            // =============================================================================================

            var chromaApiUrl = builder.Configuration["ExternalServices:ChromaDbUrl"];

            // Register CollectionsRepository
            builder.Services.AddSingleton<ICollectionsRepository>(provider =>
            {
                var options = new ChromaConfigurationOptions(chromaApiUrl);
                var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
                var chromaClient = new ChromaClient(options, httpClient);
                return new CollectionsRepository(chromaClient);
            });

            // Register EmbeddingsRepository with HttpClientFactory
            builder.Services.AddHttpClient<IEmbeddingsRepository, EmbeddingsRepository>((provider, client) =>
            {
                return new EmbeddingsRepository(chromaApiUrl, provider);
            });


            builder.Services.Configure<EmbeddingModelSettings>(
                builder.Configuration.GetSection("EmbeddingModelSettings"));

            builder.Services.AddSingleton<EmbeddingServiceFactory>();
            builder.Services.AddSingleton<EmbeddingService, OpenAIEmbeddingService>();
            builder.Services.AddScoped<ProcessedDocumentService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (enableSwagger)
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
