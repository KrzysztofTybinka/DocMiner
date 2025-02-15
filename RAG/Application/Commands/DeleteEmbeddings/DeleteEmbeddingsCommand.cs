using Domain.Abstractions;
using Infrastructure.Abstractions;
using MediatR;


namespace Application.Commands.DeleteEmbeddings
{
    public class DeleteEmbeddingsCommand : IRequest<Result>
    {
        public string CollectionName { get; set; }
        public string[]? Ids { get; set; }
    }
}
