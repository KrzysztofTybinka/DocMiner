using Domain.Abstractions;
using System.Security.Cryptography.X509Certificates;

namespace Domain.Document
{
    public class Document
    {
        public string Name { get; }
        public bool IsProcessed { get; set; }
        public string StorageLocation { get; set; }

        private Document(string name, bool isProcessed, string storageLocation)
        {
            Name = name;
            IsProcessed = isProcessed;
            StorageLocation = storageLocation;
        }

        public static Result<Document> Create(string name, string storageLocation, bool isProcessed = false)
        {
            if (string.IsNullOrEmpty(name))
                return Result<Document>.Failure(DocumentErrors.NameEmpty);

            if (string.IsNullOrEmpty(name))
                return Result<Document>.Failure(DocumentErrors.StorageLocationEmpty);

            return Result<Document>.Success(
                new Document(name, isProcessed, storageLocation));
        }
    }
}
