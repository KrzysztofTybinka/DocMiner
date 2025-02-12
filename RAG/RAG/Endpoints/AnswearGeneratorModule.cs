
using Application.Commands.GenerateResponse;
using MediatR;
using RAG.Abstractions;

namespace RagApi.Endpoints
{
    public static class AnswearGeneratorModule
    {
        public static void AddAnswearGeneratorEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/generate-response", async ([AsParameters] GenerateResponseCommand request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);

                return result.IsSuccess ?
                    Results.Ok(result.Data) :
                    result.ToProblemDetails();
            });
        }
    }
}
