
using Domain.Abstractions;

namespace Domain.ProcessedDocument
{
    public interface IProcessedDocumentRepository
    {
        Task<Result<ProcessedDocument>> Create(byte[] fileBytes, string fileName);
    }
}
