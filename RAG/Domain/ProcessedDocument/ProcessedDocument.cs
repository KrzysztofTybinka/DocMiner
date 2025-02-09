using Domain.Abstractions;

namespace Domain.ProcessedDocument
{
    public class ProcessedDocument
    {
        public string Name { get; private set; }

        public string Content { get; private set; }

        private ProcessedDocument(string name, string content) 
        {
            Name = name;
            Content = content;
        }

        public static Result<ProcessedDocument> Create(string name, string content)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Result<ProcessedDocument>
                    .Failure(ProcessedDocumentError.NameEmpty);
            }
            if (string.IsNullOrEmpty(content))
            {
                return Result<ProcessedDocument>
                    .Failure(ProcessedDocumentError.NoContent);

            }

            var result = new ProcessedDocument(name, content);
            return Result<ProcessedDocument>.Success(result);
        }
    }
}
