using Domain.Abstractions;
using Domain.ProcessedDocument;


namespace Application.Commands.ProcessDocument
{
    public class ProcessDocumentCommand
    {
        private IProcessedDocumentGenerator _processedDocumentGenerator;

        public ProcessDocumentCommand(IProcessedDocumentGenerator repository)
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
