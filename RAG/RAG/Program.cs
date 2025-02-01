
using ChromaDB.Client;
using Domain.Document;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Database;
using Persistance.Repositories;
using Persistance.Services.FileStorageService;
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

            builder.Services.AddScoped<OcrService>();

            var tesseractUrl = builder.Configuration["OCR_URL"] ?? "http://host.docker.internal:8081";

            builder.Services.AddHttpClient("ocr", client =>
            {
                client.BaseAddress = new Uri(tesseractUrl);
            });

            var chromaApiUrl = builder.Configuration.GetValue<string>("ChromaDbUrl") ?? "http://host.docker.internal:8000/api/v1/";

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

            //File storage option
            builder.Services.Configure<FileStorageSettings>(
                builder.Configuration.GetSection("FileStorageSettings"));

            //File storage
            builder.Services.AddSingleton<FileStorageFactory>();

            builder.Services.AddScoped(provider =>
            {
                var factory = provider.GetRequiredService<FileStorageFactory>();
                return factory.CreateFileStorageService();
            });

            //DocMinerDb connection
            var connectionString = builder.Configuration.GetConnectionString("DocMinerDbConnection");

            //Inject connection string
            builder.Services.AddDbContext<DocMinerDbContext>(options =>
                options.UseSqlite(connectionString));

            // Repository
            builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (enableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapPost("/DocumentStorage", (IDocumentRepository repository, DocMinerDbContext dbContext, IFileStorageService service) =>
            {
                var repo = new DocumentRepository(service, dbContext);
            });

            app.AddDocumentCollectionEndpoints();
            app.AddEmbeddingsEndpoints();

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.Run();
        }
    }
}
