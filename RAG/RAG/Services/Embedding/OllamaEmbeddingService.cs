using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RAG.ApiResponses;
using RAG.Common;
using RAG.Models;
using System.Text;

namespace RAG.Services.Embedding
{
    public class OllamaEmbeddingService : EmbeddingService
    {
        private readonly HttpClient _httpClient;

        public OllamaEmbeddingService(IOptions<EmbeddingModelSettings> embeddingModelSettings) : base(embeddingModelSettings)
        {
            _httpClient = new HttpClient();
        }


        public override async Task<Result<IEnumerable<DocumentChunk>>> CreateEmbeddingsAsync(IEnumerable<string> chunks)
        {
            var requestPayload = new
            {
                model = _embeddingModelSettings.ModelName,
                input = chunks
            };

            var jsonPayload = JsonConvert.SerializeObject(requestPayload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_embeddingModelSettings.Url, content);

            if (!response.IsSuccessStatusCode)
            {
                return Result<IEnumerable<DocumentChunk>>.Failure(
                    $"Error: {response.StatusCode}, " +
                    $"{await response.Content.ReadAsStringAsync()}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var ollamaResponse = JsonConvert.DeserializeObject<OllamaEmbeddingResponse>(jsonResponse);

            if (ollamaResponse == null)
                return Result<IEnumerable<DocumentChunk>>.Failure("Response was empty.");

            var embeddings = new List<DocumentChunk>();
            foreach (var dataItem in ollamaResponse.Data)
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
