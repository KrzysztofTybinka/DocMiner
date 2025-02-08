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

namespace Infrastructure.Services
{
    public class OcrService : IProcessedDocumentRepository
    {
        private readonly HttpClient _httpClient;

        public OcrService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<ProcessedDocument>> Create(byte[] fileBytes, string fileName)
        {
            var fileContent = new ByteArrayContent(fileBytes);

            using var multipartContent = new MultipartFormDataContent
            {
                { fileContent, "file", fileName }
            };

            var response = await _httpClient.PostAsync("/ocr", multipartContent);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                return Result<ProcessedDocument>.Failure(null);

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
