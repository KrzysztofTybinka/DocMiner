using Application.Commands.CreateEmbeddings;
using Application.Commands.DeleteEmbeddings;
using Application.Common;
using Application.Queries.GetSimilarEmbeddings;
using MediatR;
using RAG.Abstractions;
using RAG.Requests;

namespace RAG.Endpoints
{
    public static class EmbeddingsModule
    {
        public static void AddEmbeddingsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/embeddings", async ([AsParameters] CreateEmbeddingRequest request, IMediator mediator) =>
            {
                using var memoryStream = new MemoryStream();
                await request.File.CopyToAsync(memoryStream);
                var fileData = new FileData
                {
                    FileName = request.File.FileName,
                    Content = memoryStream.ToArray()
                };

                var command = new CreateEmbeddingsCommand
                {
                    File = fileData,
                    CollectionName = request.CollectionName,
                    ProcessedDocumentService = request.ProcessedDocumentService,
                    EmbeddingGeneratorFactory = request.EmbeddingGeneratorFactory,
                    EmbeddingsRepositoryFactory = request.EmbeddingsRepositoryFactory,
                    NumberOfTokens = request.NumberOfTokens
                };

                var result = await mediator.Send(command);

                return result.IsSuccess ?
                       Results.Ok("Embeddings created.") :
                       result.ToProblemDetails();
            }).DisableAntiforgery();

            app.MapPost("/query-embeddings", async ([AsParameters] GetSimilarEmbeddingsQuery request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);

                return result.IsSuccess ?
                    Results.Ok(result.Data) :
                    result.ToProblemDetails();
            });

            app.MapGet("/embeddings", async ([AsParameters] GetEmbeddingsQuery request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);

                return result.IsSuccess ?
                    Results.Ok(result.Data) :
                    result.ToProblemDetails();
            });

            app.MapDelete("/embeddings", async ([AsParameters] DeleteEmbeddingsCommand request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);

                return result.IsSuccess ?
                    Results.Ok("Embeddings deleted.") :
                    result.ToProblemDetails();
            });
        }
        
    }
}
