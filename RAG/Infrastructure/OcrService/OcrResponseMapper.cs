using Domain.Abstractions;
using Domain.ProcessedDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.OcrService
{
    public static class OcrResponseMapper
    {
        public static Result<ProcessedDocument> ToDomainProcessedDocument(this OcrResponse response, string fileName)
        {
            return ProcessedDocument.Create(fileName, response.Text);
        }
    }
}
