
using Domain.Abstractions;
using Domain.Document;

namespace Application.Services
{
    public class DocumentService
    {
        private IDocumentRepository _repository;

        public DocumentService(IDocumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Document>> GetDocumentAsync(string documentName)
        {
            return await _repository.GetDocumentAsync(documentName);
        }

        public async Task<Result> UploadDocumentAsync(Document document)
        {
            return await _repository.UploadDocumentAsync(document);
        }

        public async Task<Result> DeleteDocumentAsync(string documentName)
        {
            return await _repository.DeleteDocumentAsync(documentName);
        }
    }
}
