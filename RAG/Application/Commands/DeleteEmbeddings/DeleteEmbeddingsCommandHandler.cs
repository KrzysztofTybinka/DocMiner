
using Domain.Abstractions;
using MediatR;

namespace Application.Commands.DeleteEmbeddings
{
    public class DeleteEmbeddingsCommandHandler : IRequestHandler<DeleteEmbeddingsCommand, Result>
    {
        public async Task<Result> Handle(DeleteEmbeddingsCommand request, CancellationToken cancellationToken)
        {
            //Get propper collection
            var embeddingsRepositoryResult = await request
                .EmbeddingsRepositoryFactory
                .CreateRepositoryAsync(request.CollectionName);

            if (!embeddingsRepositoryResult.IsSuccess)
            {
                return Result.Failure(embeddingsRepositoryResult.Error);
            }

            var ids = request.Ids
                .Select(id => new Guid(id))
                .ToList();

            var embeddingsRepository = embeddingsRepositoryResult.Data;
            var deleteResult = await embeddingsRepository.DeleteEmbeddingsAsync(ids);

            if (!deleteResult.IsSuccess)
            {
                return Result.Failure(deleteResult.Error);
            }

            return Result.Success();
        }
    }
}
