using Application.Abstractions;
using Application.Common;
using Application.Responses;
using Application.Services;
using Domain.Abstractions;
using Infrastructure.Abstractions;
using MediatR;

namespace Application.Commands.CreateEmbeddings
{
    public class CreateEmbeddingsCommand : IRequest<Result>
    {
        public FileData File { get; set; }
        public string CollectionName { get; set; }
        public ProcessedDocumentService ProcessedDocumentService { get; set; }
        public IEmbeddingGeneratorFactory EmbeddingGeneratorFactory { get; set; }
        public IEmbeddingRepositoryFactory EmbeddingsRepositoryFactory { get; set; }
        public int NumberOfTokens { get; set; } = 50;
    }
}
