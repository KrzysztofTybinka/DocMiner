
using Domain.Abstractions;
using Domain.Embedings;
using Infrastructure.Configuration;
using Infrastructure.Responses;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace Infrastructure.Services.EmbeddingService
{
    public class OllamaEmbeddingService : IEmbeddingGenerator
    {
        private readonly EmbeddingModelSettings _settings;
        private readonly HttpClient _httpClient;

        public OllamaEmbeddingService(IOptions<EmbeddingModelSettings> embeddingModelSettings,
            IHttpClientFactory httpClientFactory)
        {
            _settings = embeddingModelSettings.Value;
            _httpClient = httpClientFactory.CreateClient("EmbeddingModelClient");
        }

        public async Task<Result<Embedding>> GenerateEmbeddingAsync(string text)
        {
            var result = await CallOllamaApi([text]);
            return result.IsSuccess
                ? Result<Embedding>.Success(result.Data.First())
                : Result<Embedding>.Failure(result.Error);
        }

        public async Task<Result<List<Embedding>>> GenerateEmbeddingsAsync(List<string> textPieces)
            => await CallOllamaApi(textPieces);

        public async Task<Result<List<Embedding>>> CallOllamaApi(List<string> textPieces)
        {
            var requestPayload = new
            {
                model = _settings.ModelName,
                input = textPieces
            };

            var jsonPayload = JsonConvert.SerializeObject(requestPayload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_settings.Url, content);

            if (!response.IsSuccessStatusCode)
            {
                return Result<List<Embedding>>.Failure(EmbeddingServiceErrors
                    .CouldntProcess(_settings.ModelName,
                    await response.Content.ReadAsStringAsync()));
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var ollamaResponse = JsonConvert.DeserializeObject<OllamaCreateEmbeddingsResponse>(jsonResponse);

            if (ollamaResponse == null)
                return Result<List<Embedding>>.Failure(EmbeddingServiceErrors.EmptyResponse);

            var embeddings = new List<Embedding>();

            foreach (var dataItem in ollamaResponse.Data)
            {
                var embedding = Embedding.Create(
                    Guid.NewGuid(),
                    textPieces[dataItem.Index],
                    dataItem.Embedding.ToArray());

                if (!embedding.IsSuccess)
                {
                    return Result<List<Embedding>>.Failure(embedding.Error);
                }

                embeddings.Add(embedding.Data);
            }

            return Result<List<Embedding>>.Success(embeddings);
        }
    }
}
