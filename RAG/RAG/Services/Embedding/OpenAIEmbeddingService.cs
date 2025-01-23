using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RAG.ApiResponses;
using RAG.Common;
using RAG.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RAG.Services.Embedding
{
    public class OpenAIEmbeddingService : EmbeddingService
    {
        private readonly HttpClient _httpClient;

        public OpenAIEmbeddingService(IOptions<EmbeddingModelSettings> embeddingModelSettings) : base(embeddingModelSettings) 
        {
            _httpClient = new HttpClient();
        }

        public override async Task<Result<IEnumerable<DocumentChunk>>> CreateEmbeddingsAsync(IEnumerable<string> chunks)
        {
            var requestPayload = new
            {
                model = _embeddingModelSettings.ModelName,
                input = chunks,
                encoding_format = "float"
            };

            var jsonPayload = JsonConvert.SerializeObject(requestPayload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_embeddingModelSettings.Token}");

            var response = await _httpClient.PostAsync(_embeddingModelSettings.Url, content);

            if (!response.IsSuccessStatusCode)
            {
                return Result<IEnumerable<DocumentChunk>>.Failure(
                    $"Error: {response.StatusCode}, " +
                    $"{await response.Content.ReadAsStringAsync()}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var openAIResponse = JsonConvert.DeserializeObject<OpenAIEmbeddingResponse>(jsonResponse);

            if (openAIResponse == null)
                return Result<IEnumerable<DocumentChunk>>.Failure("Response was empty.");

            var embeddings = new List<DocumentChunk>();
            foreach (var dataItem in openAIResponse.Data)
            {
                embeddings.Add(new DocumentChunk
                {
                    EmbeddingVector = dataItem.Embedding,
                    Chunk = chunks.ToList()[dataItem.Index],
                });
            }

            return Result<IEnumerable<DocumentChunk>>.Success(embeddings);
        }
    }
}
