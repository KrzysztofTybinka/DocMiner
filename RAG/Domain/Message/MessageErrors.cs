
using Domain.Abstractions;

namespace Domain.Message
{
    public class MessageErrors
    {
        public static Error QuestionEmpty => Error.Failure(
            "MessageErrors.QuestionEmpty",
            "A question was empty."
        );

        public static Error RetrievedDataEmpty => Error.Failure(
            "MessageErrors.RetrievedDataEmpty",
            "Retrieved data was empty."
        );

        public static Error PromptWrapperEmpty => Error.Failure(
            "MessageErrors.PromptWrapperEmpty",
            "A prompt wrapper was empty."
        );

        public static Error InvalidPromptWrapper(string placeholder) => Error.Failure(
            "MessageErrors.InvalidPromptWrapper",
            $"The prompt wrapper does not contain " +
            $"the required placeholders: {placeholder}."
        );
    }
}
