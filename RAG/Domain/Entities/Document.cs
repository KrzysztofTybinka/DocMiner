using Domain.Abstractions;

namespace Domain.Entities
{
    public class Document
    {
        public string Name { get; }
        public string Content { get; }

        private Document(string name, string content)
        {
            Name = name;
            Content = content;
        }

        public static Result<Document> Create(string name, string content)
        {
            return Result<Document>.Success(
                new Document(name, content));
        }
    }
}
