
using Application.Abstractions;
using Application.Services;
using ChromaDB.Client;
using Domain.ProcessedDocument;
using Infrastructure.Services.EmbeddingService;
using Infrastructure.Services.OcrService;
using RAG.Endpoints;
using RAG.Repository;


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

            // Register Embedding service client ===========================================================
            // Register the HTTP client for Ollama
            builder.Services.AddHttpClient("EmbeddingModelClient", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["EmbeddingModelSettings:Url"]);
            });

            // Register the factory and settings
            builder.Services.AddScoped<IEmbeddingGeneratorFactory, EmbeddingServiceFactory>();

            builder.Services.Configure<Infrastructure.Configuration.EmbeddingModelSettings>(
                builder.Configuration.GetSection("EmbeddingModelSettings")
            );
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
