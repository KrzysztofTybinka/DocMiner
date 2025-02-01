using Domain.Abstractions;

namespace Domain.ProcessedDocument
{
    public class ProcessedDocument
    {
        public string Name { get; }
        public string Content { get; }

        private ProcessedDocument(string name, string content)
        {
            Name = name;
            Content = content;
        }

        public static Result<ProcessedDocument> Create(string name, string content)
        {
            if (string.IsNullOrEmpty(name))
                return Result<ProcessedDocument>.Failure(ProcessedDocumentErrors.NameEmpty);

            if (string.IsNullOrEmpty(content))
                return Result<ProcessedDocument>.Failure(ProcessedDocumentErrors.ContentEmpty);

            return Result<ProcessedDocument>.Success(
                new ProcessedDocument(name, content));
        }
    }
}
