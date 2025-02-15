using Application.Common;
using Domain.Abstractions;
using MediatR;

namespace Application.Commands.CreateEmbeddings
{
    public class CreateEmbeddingsCommand : IRequest<Result>
    {
        public FileData File { get; set; }
        public string CollectionName { get; set; }
        public int NumberOfTokens { get; set; } = 50;
    }
}
