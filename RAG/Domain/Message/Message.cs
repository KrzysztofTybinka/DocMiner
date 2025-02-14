

using Domain.Abstractions;

namespace Domain.Message
{
    public class Message
    {
        public string Content { get; }

        private Message(string content)
        {
            Content = content;
        }

        /// <summary>
        ///Creates a formatted prompt.
        /// </summary>
        /// <param name="question">A question in regard to passed data</param>
        /// <param name="retrievedData">The data retrieved from the document</param>
        /// <param name="promptWrapper">Prompt helper that glues retrieved data and a question together</param>
        /// <returns>Message with a formatted prompt.</returns>
        /// 
        /// User is expected to pass prompt wrapper in a format
        /// that will produce the final message after replacement.
        public static Result<Message> Create(string question, string retrievedData, string promptWrapper)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                return Result<Message>.Failure(MessageErrors.QuestionEmpty);
            }

            if (string.IsNullOrWhiteSpace(retrievedData))
            {
                return Result<Message>.Failure(MessageErrors.RetrievedDataEmpty);
            }

            if (string.IsNullOrWhiteSpace(promptWrapper))
            {
                return Result<Message>.Failure(MessageErrors.PromptWrapperEmpty);
            }

            if (!promptWrapper.Contains("{retrievedData}"))
            {
                return Result<Message>.Failure(MessageErrors.InvalidPromptWrapper("{retrievedData}"));
            }

            if (!promptWrapper.Contains("{question}"))
            {
                return Result<Message>.Failure(MessageErrors.InvalidPromptWrapper("{question}"));
            }

            string formattedPrompt = promptWrapper
                .Replace("{retrievedData}", retrievedData)
                .Replace("{question}", question);

            var result = new Message(formattedPrompt);

            return Result<Message>.Success(result);
        }
    }
}
