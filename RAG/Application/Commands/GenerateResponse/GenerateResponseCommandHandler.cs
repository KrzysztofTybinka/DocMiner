

using Application.Abstractions;
using Application.Commands.CreateEmbeddings;
using Domain.Abstractions;
using Domain.Message;
using MediatR;

namespace Application.Commands.GenerateResponse
{
    public class GenerateResponseCommandHandler : IRequestHandler<GenerateResponseCommand, Result<string>>
    {
        private IAnswearGeneratorFactory _answearGeneratorFactory { get; set; }
        public GenerateResponseCommandHandler(IAnswearGeneratorFactory answearGeneratorFactory)
        {
            _answearGeneratorFactory = answearGeneratorFactory;
        }

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
            var generator = _answearGeneratorFactory
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
