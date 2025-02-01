using Domain.Abstractions;
using Domain.Document;

namespace Application.Document
{
    public class DocumentService
    {
        private IDocumentRepository _documentRepository;

        public DocumentService(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<Result> CreateAsync(Domain.Document.Document document)
        {
            return await _documentRepository.Create(document);
        }
    }
}
