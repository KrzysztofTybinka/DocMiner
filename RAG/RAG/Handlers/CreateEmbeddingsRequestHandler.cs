using RAG.BLL.Chunking;
using RAG.Requests;
using RAG.Services.Embedding;
using RAG.Common;
using RAG.Models;
using RAG.Abstractions;

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

            var result = await request.ProcessedDocumentService.CreateChunksAsync(bytes,
                request.File.FileName,
                request.NumberOfTokens);

            if (!result.IsSuccess)
                return result.ToProblemDetails();

            //Get embedding service
            EmbeddingService embeddingModel = request.EmbeddingServiceFactory.CreateEmbeddingModel();

            //Create embeddings
            var embeddingsResult = await embeddingModel.CreateEmbeddingsAsync(result.Data);

            //Add metadata and ids to chunks
            string fileName = Path.GetFileNameWithoutExtension(request.File.FileName);
            var documentChunks = ChunkDataFiller.Fill(embeddingsResult.Data.ToList(), fileName);

            //Save embeddings into db
            var collection = await request.CollectionsRepository.GetDocumentCollection(request.CollectionName);
            await request.EmbeddingsRepository.UploadDocument(embeddingsResult.Data, collection);

            return Results.Ok("Embeddings created.");
        }
    }
}
