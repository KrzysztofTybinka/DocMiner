
using Domain.Abstractions;
using Domain.Embedings;

namespace Domain.Document
{
    public class Document
    {
        public string Name { get; set; }

        public List<Embedding> Embeddings { get; set; }

        private Document(string name, List<Embedding> embeddings)
        {
            Name = name;
            Embeddings = embeddings;
        }

        public static Result<Document> Create(string name, List<Embedding> embeddings)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Result<Document>.Failure(DocumentErrors.NameEmpty);
            }
            if (embeddings == null || embeddings.Count == 0)
            {
                return Result<Document>.Failure(DocumentErrors.EmbeddingsCollectionEmpty);
            }

            var result = new Document(name, embeddings);
            return Result<Document>.Success(result);
        }
    }
}
