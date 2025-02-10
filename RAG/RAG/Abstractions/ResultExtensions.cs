using Domain.Abstractions;

namespace RAG.Abstractions
{
    public static class ResultExtensions
    {
        public static IResult ToProblemDetails(this Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException();
            }

            return CreateProblemDetails(result.Error);
        }

        public static IResult ToProblemDetails<T>(this Result<T> result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException();
            }

            return CreateProblemDetails(result.Error);
        }

        private static IResult CreateProblemDetails(Error error)
        {
            return Results.Problem(
                statusCode: GetStatusCode(error.ErrorType),
                title: GetTitle(error.ErrorType),
                extensions: new Dictionary<string, object?>
                {
                { "errors", new[] { error } }
                });
        }

        private static int GetStatusCode(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.Failure => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status401Unauthorized,
                ErrorType.ExternalServiceFailure => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

        private static string GetTitle(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.Failure => "Bad Request",
                ErrorType.NotFound => "Not Found",
                ErrorType.Conflict => "Conflict",
                ErrorType.Validation => "Validation Error",
                ErrorType.ExternalServiceFailure => "Server Failure",
                _ => "Server Failure"
            };
    }
}
