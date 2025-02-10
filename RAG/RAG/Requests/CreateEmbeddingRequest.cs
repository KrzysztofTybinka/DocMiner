﻿using Application.Abstractions;
using Application.Services;
using Domain.ProcessedDocument;
using Infrastructure.Abstractions;
using Infrastructure.Repositories.ChromaCollection;
using Infrastructure.Services;

namespace RAG.Requests
{
    public class CreateEmbeddingRequest
    {
        public IFormFile File {  get; set; }
        public string CollectionName { get; set; }
        public ProcessedDocumentService ProcessedDocumentService { get; set; }
        public IEmbeddingGeneratorFactory EmbeddingGeneratorFactory { get; set; }
        public IEmbeddingRepositoryFactory EmbeddingsRepositoryFactory { get; set; }
        public int NumberOfTokens { get; set; } = 50;
    }
}
