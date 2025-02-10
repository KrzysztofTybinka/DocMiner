using RAG.Requests;
using RAG.Abstractions;
using Domain.Embedings;
using Infrastructure.Repositories.DocumentRepository;

namespace RAG.Handlers
{
    public static class CreateEmbeddingsRequestHandler
    {
        public static async Task<IResult> Handle(this CreateEmbeddingRequest request)
        {
            //Get propper collection
            var embeddingsRepositoryResult = await request
                .EmbeddingsRepositoryFactory
                .CreateRepositoryAsync(request.CollectionName);

            if (!embeddingsRepositoryResult.IsSuccess)
            {
                return embeddingsRepositoryResult.ToProblemDetails();
            }

            //Ocr the file
            using var memoryStream = new MemoryStream();
            await request.File.CopyToAsync(memoryStream);
            var bytes = memoryStream.ToArray();

            var chunksResult = await request.ProcessedDocumentService
                .CreateChunksAsync(bytes,
                    request.File.FileName,
                    request.NumberOfTokens);

            if (!chunksResult.IsSuccess)
                return chunksResult.ToProblemDetails();

            //Get embedding service
            var embeddingModel = request.EmbeddingGeneratorFactory
                .CreateEmbeddingGenerator();

            //Create embeddings
            var embeddingResult = await embeddingModel
                .GenerateEmbeddingsAsync(chunksResult.Data);

            if (!embeddingResult.IsSuccess)
                return embeddingResult.ToProblemDetails();

            //Assign source name
            string fileName = Path.GetFileNameWithoutExtension(request.File.FileName);

            embeddingResult.Data.ForEach(e => 
                e.Details = new EmbeddingDetails() 
                {
                    Source = fileName 
                });

            //Save embeddings into a collection
            var embeddingsRepository = embeddingsRepositoryResult.Data;
            await embeddingsRepository.UploadEmbeddingsAsync(embeddingResult.Data);

            return Results.Ok("Embeddings created.");
        }
    }
}
