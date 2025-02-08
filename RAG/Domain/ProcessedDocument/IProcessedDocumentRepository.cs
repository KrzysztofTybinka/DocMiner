
using Domain.Abstractions;

namespace Domain.ProcessedDocument
{
    public interface IProcessedDocumentRepository
    {
        Task<Result<ProcessedDocument>> Create(string name, string content);
    }
}
