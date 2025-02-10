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
        private IProcessedDocumentGenerator _processedDocumentGenerator;

        public ProcessedDocumentService(IProcessedDocumentGenerator repository)
        {
            _processedDocumentGenerator = repository;
        }

        public async Task<Result<ProcessedDocument>> CreateAsync(
            byte[] fileBytes, 
            string fileName)
        {
            return await _processedDocumentGenerator.ProcessDocument(fileBytes, fileName);
        }

        public async Task<Result<List<string>>> CreateChunksAsync(
            byte[] fileBytes, 
            string fileName, 
            int tokenAmount)
        {
            var processedDocument = await _processedDocumentGenerator.ProcessDocument(fileBytes, fileName);

            if (!processedDocument.IsSuccess)
            {
                return Result<List<string>>.Failure(processedDocument.Error);
            }

            string[] delimiters = { ";", ".", "?", "!" };
            var chunks = DocumentChunker.ChunkContent(
                processedDocument.Data.Content,
                tokenAmount,
                delimiters
            );

            return chunks;
        }
    }
}
