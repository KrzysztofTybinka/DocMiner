
using Application.Abstractions;
using Application.Services;
using Domain.Abstractions;
using MediatR;

namespace Application.Commands.GenerateResponse
{
    public class GenerateResponseCommand : IRequest<Result<string>>
    {
        public string Question { get; set; }
        public string RetrievedData { get; set; }
        public string PromptWrapper { get; set; }
        public IAnswearGeneratorFactory AnswearGeneratorFactory { get; set; }
        public Dictionary<string, object>? Parameters { get; set; }
    }
}
