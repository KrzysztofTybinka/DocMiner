
using Domain.Abstractions;

namespace Domain.Document
{
    public interface IDocumentRepository
    {
        Task<Result<List<Document>>> QueryDocumentsAsync();
        Task<Result<Document>> GetDocumentAsync(string documentName);
        Task<Result> UploadDocumentAsync(Document document);
        Task<Result> DeleteDocumentAsync(string documentName);
    }
}
