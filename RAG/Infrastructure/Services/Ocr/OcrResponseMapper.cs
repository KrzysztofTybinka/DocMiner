
using Domain.Abstractions;
using Domain.ProcessedDocument;

namespace Infrastructure.Services.OcrService.Ocr
{
    public static class OcrResponseMapper
    {
        public static Result<ProcessedDocument> ToDomainProcessedDocument(this OcrResponse response, string fileName)
        {
            return ProcessedDocument.Create(fileName, response.Text);
        }
    }
}
