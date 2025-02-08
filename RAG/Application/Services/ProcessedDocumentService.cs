using Domain.Abstractions;
using Domain.ProcessedDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProcessedDocumentService
    {
        private IProcessedDocumentRepository _repository;

        public ProcessedDocumentService(IProcessedDocumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<ProcessedDocument>> CreateAsync(string name, string content)
        {
            return await _repository.Create(name, content);
        }
    }
}
