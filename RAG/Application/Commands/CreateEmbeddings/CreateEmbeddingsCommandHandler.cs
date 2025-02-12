using Domain.Abstractions;
using Domain.Embedings;
using MediatR;


namespace Application.Commands.CreateEmbeddings
{
    public class CreateEmbeddingsCommandHandler : IRequestHandler<CreateEmbeddingsCommand, Result>
    {
        public async Task<Result> Handle(CreateEmbeddingsCommand request, CancellationToken cancellationToken)
        {
            //Get propper collection
            var embeddingsRepositoryResult = await request
                .EmbeddingsRepositoryFactory
                .CreateRepositoryAsync(request.CollectionName);

            if (!embeddingsRepositoryResult.IsSuccess)
            {
                return Result.Failure(embeddingsRepositoryResult.Error);
            }

            //Ocr the file
            var bytes = request.File.Content;

            var chunksResult = await request.ProcessedDocumentService
                .CreateChunksAsync(bytes,
                    request.File.FileName,
                    request.NumberOfTokens);

            if (!chunksResult.IsSuccess)
                return Result.Failure(chunksResult.Error);

            //Get embedding service
            var embeddingModel = request.EmbeddingGeneratorFactory
                .CreateEmbeddingGenerator();

            //Create embeddings
            var embeddingResult = await embeddingModel
                .GenerateEmbeddingsAsync(chunksResult.Data);

            if (!embeddingResult.IsSuccess)
                return Result.Failure(embeddingResult.Error);

            //Assign source name
            string fileName = Path.GetFileNameWithoutExtension(request.File.FileName);

            embeddingResult.Data.ForEach(e =>
                e.Details = new EmbeddingDetails()
                {
                    Source = fileName
                });

            //Save embeddings into a collection
            var embeddingsRepository = embeddingsRepositoryResult.Data;
            var result = await embeddingsRepository
                .UploadEmbeddingsAsync(embeddingResult.Data);

            if (!result.IsSuccess)
            {
                return Result.Failure(result.Error);
            }

            return Result.Success();
        }
    }
}
