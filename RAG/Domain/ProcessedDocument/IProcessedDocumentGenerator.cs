
using Domain.Abstractions;

namespace Domain.ProcessedDocument
{
    public interface IProcessedDocumentGenerator
    {
        Task<Result<ProcessedDocument>> ProcessDocument(byte[] fileBytes, string fileName);
    }
}
