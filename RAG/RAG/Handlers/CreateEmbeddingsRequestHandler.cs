using RAG.Requests;
using RAG.Abstractions;
using Domain.Document;

namespace RAG.Handlers
{
    public static class CreateEmbeddingsRequestHandler
    {
        public static async Task<IResult> Handle(this CreateEmbeddingRequest request)
        {
            //Ocr the file
            using var memoryStream = new MemoryStream();
            await request.File.CopyToAsync(memoryStream);
            var bytes = memoryStream.ToArray();

            var chunksResult = await request.ProcessedDocumentService.CreateChunksAsync(
                bytes,
                request.File.FileName,
                request.NumberOfTokens);

            if (!chunksResult.IsSuccess)
                return chunksResult.ToProblemDetails();

            //Get embedding service
            var embeddingModel = request.EmbeddingGeneratorFactory.CreateEmbeddingGenerator();

            //Create embeddings
            var embeddingResult = await embeddingModel.GenerateEmbeddingsAsync(chunksResult.Data);

            if (!embeddingResult.IsSuccess)
                return embeddingResult.ToProblemDetails();

            //Create document
            string fileName = Path.GetFileNameWithoutExtension(request.File.FileName);
            var documentResult = Document.Create(fileName, embeddingResult.Data);

            if (!documentResult.IsSuccess)
                return documentResult.ToProblemDetails();

            //Save embeddings into db
            var collection = await request.CollectionsRepository.GetDocumentCollection(request.CollectionName);
            await request.EmbeddingsRepository.UploadDocument(embeddingResult.Data, collection);

            return Results.Ok("Embeddings created.");
        }
    }
}
