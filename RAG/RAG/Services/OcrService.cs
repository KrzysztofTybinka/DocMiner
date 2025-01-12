using RAG.Common;
using RAG.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RAG.Services
{
    public class OcrService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OcrService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Result<OcrResponse>> RequestOCRAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var fileBytes = memoryStream.ToArray();
            var fileContent = new ByteArrayContent(fileBytes);

            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            using var multipartContent = new MultipartFormDataContent
            {
                { fileContent, "file", file.FileName }
            };

            var httpClient = new HttpClient();
            var client = _httpClientFactory.CreateClient("ocr");

            var response = await client.PostAsync("/ocr", multipartContent);


            if (!response.IsSuccessStatusCode)
                return Result<OcrResponse>.Failure("Request failed");

            string responseAsString = await response.Content.ReadAsStringAsync();

            var responseAsJson = JsonSerializer.Deserialize<OcrResponse>(responseAsString);
            return Result<OcrResponse>.Success(responseAsJson!);
        }
    }
}
