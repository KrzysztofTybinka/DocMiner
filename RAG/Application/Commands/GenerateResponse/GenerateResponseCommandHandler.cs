

using Application.Commands.CreateEmbeddings;
using Domain.Abstractions;
using Domain.Message;
using MediatR;

namespace Application.Commands.GenerateResponse
{
    public class GenerateResponseCommandHandler : IRequestHandler<GenerateResponseCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(GenerateResponseCommand request, CancellationToken cancellationToken)
        {
            //Create message
            var message = Message.Create(
                question: request.Question, 
                retrievedData: request.RetrievedData, 
                promptWrapper: request.PromptWrapper);

            if (!message.IsSuccess)
            {
                return Result<string>.Failure(message.Error);
            }

            //Create proper generator
            var generator = request.AnswearGeneratorFactory
                .CreateAnswearGenerator(request.Parameters);

            //Ask language model a question
            var response = await generator.GenerateAnswearAsync(message.Data);

            if (!response.IsSuccess)
            {
                return Result<string>.Failure(response.Error);
            }

            return Result<string>.Success(response.Data);
        }
    }
}
