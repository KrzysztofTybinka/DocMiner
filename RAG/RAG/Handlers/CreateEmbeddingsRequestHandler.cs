﻿using RAG.BLL.Chunking;
using RAG.Requests;
using RAG.Services.Embedding;
using RAG.Common;

namespace RAG.Handlers
{
    public static class CreateEmbeddingsRequestHandler
    {
        public static async Task<Result> Handle(this CreateEmbeddingRequest request)
        {
            //Ocr the file
            var result = await request.OcrService.RequestOCRAsync(request.File);

            if (!result.IsSuccess)
                return Result.Failure(result.ErrorMessage);

            //Clean up result
            string cleanedResult = result.Data.Text.Replace("\r\n", " ").Replace("\n", " ");

            //Split file into chunks
            Chunker chunker = new(request.NumberOfTokens, cleanedResult);
            List<string> chunks = chunker.GetChunks();

            //Get embedding service
            EmbeddingService embeddingModel = request.EmbeddingServiceFactory.CreateEmbeddingModel();

            //Create embeddings
            string fileName = Path.GetFileNameWithoutExtension(request.File.FileName);
            var embeddingsResult = await embeddingModel.CreateEmbeddingsAsync(chunks, fileName);

            if (!embeddingsResult.IsSuccess)
                return Result.Failure(result.ErrorMessage);

            //Save embeddings into db
            var uploadResult = await request.Repository.UploadDocument(embeddingsResult.Data, request.DocumentCollectionId);

            return uploadResult;
        }
    }
}
