
namespace Domain.Abstractions
{
    public record Error
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

        private Error(string code, string description, ErrorType errorType)
        {
            Code = code;
            Description = description;
            ErrorType = errorType;
        }

        public string Code { get; set; }
        public string Description { get; set; }
        public ErrorType ErrorType { get; set; }

        public static Error Failure(string code, string description) =>
            new(code, description, ErrorType.Failure);

        public static Error Validation(string code, string description) =>
            new(code, description, ErrorType.Validation);

        public static Error NotFound(string code, string description) =>
            new(code, description, ErrorType.NotFound);

        public static Error Conflict(string code, string description) =>
            new(code, description, ErrorType.Conflict);

        public static Error ExternalServiceFailure(string code, string description) => 
            new(code, description, ErrorType.ExternalServiceFailure);
    }

    public enum ErrorType
    {
        Failure = 0,
        Validation = 1,
        NotFound = 2,
        Conflict = 3,
        ExternalServiceFailure = 4
    }
}
