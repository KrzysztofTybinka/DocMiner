
using Application.Abstractions;
using Application.Commands.CreateEmbeddings;
using Application.Commands.DeleteEmbeddings;
using Application.Queries;
using Application.Queries.GetSimilarEmbeddings;
using Application.Services;
using ChromaDB.Client;
using Domain.ProcessedDocument;
using Infrastructure.Abstractions;
using Infrastructure.Factories;
using Infrastructure.Factories.Embeddings;
using Infrastructure.Repositories.ChromaCollection;
using Infrastructure.Services.EmbeddingService;
using Infrastructure.Services.Ocr;
using RAG.Endpoints;


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

            //Register OCR service =================================================================================
            var tesseractUrl = builder.Configuration["ExternalServices:OcrUrl"] ??
                throw new ArgumentNullException("Ocr url address was empty.");

            builder.Services.AddHttpClient<IProcessedDocumentGenerator, OcrService>(client =>
            {
                client.BaseAddress = new Uri(tesseractUrl);
            });

            // Register Embedding service client ==================================================================
            builder.Services.AddHttpClient("EmbeddingModelClient", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["EmbeddingModelSettings:Url"]!);
            });

            // Register the factory and settings
            builder.Services.AddScoped<IEmbeddingGeneratorFactory, EmbeddingServiceFactory>();

            builder.Services.Configure<Infrastructure.Configuration.EmbeddingModelSettings>(
                builder.Configuration.GetSection("EmbeddingModelSettings")
            );

            var chromaApiUrl = builder.Configuration["ExternalServices:ChromaDbUrl"];

            if (string.IsNullOrEmpty(chromaApiUrl))
                throw new ArgumentNullException(nameof(chromaApiUrl), "ChromaDb url is not configured.");

            // Register ChromaConfigurationOptions as a singleton
            builder.Services.AddSingleton(new ChromaConfigurationOptions(chromaApiUrl));

            // Register CollectionsRepository =====================================================================
            builder.Services.AddSingleton(provider =>
            {
                var options = provider.GetRequiredService<ChromaConfigurationOptions>();
                var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient();
                var chromaClient = new ChromaClient(options, httpClient);
                return new ChromaCollectionRepository(chromaClient);
            });

            // Register ChromaCollectionClientFactory =============================================================
            builder.Services.AddSingleton(provider =>
            {
                var options = provider.GetRequiredService<ChromaConfigurationOptions>();
                var httpClient = provider.GetRequiredService<IHttpClientFactory>();
                var collectionRepository = provider.GetRequiredService<ChromaCollectionRepository>();
                return new ChromaCollectionClientFactory(httpClient, options, collectionRepository);
            });

            // Register EmbeddingsRepository factory ===============================================================
            builder.Services.AddSingleton<IEmbeddingRepositoryFactory>(provider =>
            {
                return new EmbeddingRepositoryFactory(
                    provider.GetRequiredService<ChromaCollectionClientFactory>());
            });

            // Register GetSimilarEmbeddingsQueryHandler factory ==================================================
            builder.Services.AddSingleton<IGetSimilarEmbeddingsQueryHandlerFactory>(provider =>
            {
                return new GetSimilarEmbeddingsQueryHandlerFactory(
                    provider.GetRequiredService<ChromaCollectionClientFactory>());
            });

            // Register GetEmbeddingsByIdQueryHandler factory =====================================================
            builder.Services.AddSingleton<IGetEmbeddingsByIdQueryHandlerFactory>(provider =>
            {
                return new GetEmbeddingsByIdQueryHandlerFactory(
                    provider.GetRequiredService<ChromaCollectionClientFactory>());
            });

            // ====================================================================================================

            builder.Services.AddScoped<ProcessedDocumentService>();

            // MediatR ============================================================================================
            builder.Services.AddMediatR(cfg => 
                cfg.RegisterServicesFromAssembly(typeof(GetEmbeddingsByIdHandler).Assembly));

            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(GetSimilarEmbeddingsHandler).Assembly));

            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(CreateEmbeddingsCommandHandler).Assembly));

            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(DeleteEmbeddingsCommandHandler).Assembly));

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
