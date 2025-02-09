using Domain.Abstractions;
using Domain.ProcessedDocument;
using Infrastructure.OcrService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mime;

namespace Infrastructure.Services
{
    public class OcrService : IProcessedDocumentGenerator
    {
        private readonly HttpClient _httpClient;

        public OcrService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<ProcessedDocument>> ProcessDocument(byte[] fileBytes, string fileName)
        {
            using var content = new MultipartFormDataContent();

            var byteArrayContent = new ByteArrayContent(fileBytes);
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            content.Add(byteArrayContent, "file", fileName);

            var response = await _httpClient.PostAsync("/ocr", content);

            if (!response.IsSuccessStatusCode)
                return Result<ProcessedDocument>.Failure(OcrServiceErrors.CouldntProcess);

            string responseAsString = await response.Content.ReadAsStringAsync();
            var ocrResponse = JsonSerializer.Deserialize<OcrResponse>(responseAsString);

            if (ocrResponse == null)
            {
                return Result<ProcessedDocument>.Failure(OcrServiceErrors.EmptyResponse);
            }
            return ocrResponse.ToDomainProcessedDocument(fileName);
        }
    }
}
