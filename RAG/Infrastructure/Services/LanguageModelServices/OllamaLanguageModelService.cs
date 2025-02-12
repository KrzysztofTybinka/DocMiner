

using Domain.Abstractions;
using Domain.Message;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System.Text;
using Infrastructure.Services.LanguageModelServices;
using Newtonsoft.Json;
using Infrastructure.Services.LanguageModelServices.Parameters;
using Infrastructure.Responses.LanguageModels;

namespace Infrastructure.Services.AnswearGeneratorServices
{
    public class OllamaLanguageModelService : IAnswearGenerator
    {
        private readonly LanguageModelSettings _settings;
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, object>? _parameters;

        public OllamaLanguageModelService(IOptions<LanguageModelSettings> languageModelSettings,
            IHttpClientFactory httpClientFactory,
            Dictionary<string, object>? parameters)
        {
            _settings = languageModelSettings.Value;
            _httpClient = httpClientFactory.CreateClient("LanguageModelClient");
            _parameters = parameters;
        }

        public async Task<Result<string>> GenerateAnswearAsync(Message message)
        {
            var parameters = new OllamaParameters(_parameters);

            var requestBody = new
            {
                model = _settings.ModelName,
                prompt = message.Content,
                stream = false,
                temperature = parameters.Temperature,
                max_tokens = parameters.MaxTokens,
            };

            var jsonPayload = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_settings.Url, content);

            if (!response.IsSuccessStatusCode)
            {
                return Result<string>.Failure(LanguageModelErrors
                    .CouldntProcess(_settings.ModelName,
                    await response.Content.ReadAsStringAsync()));
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var ollamaResponse = JsonConvert.DeserializeObject<OllamaLanguageModelResponse>(jsonResponse);

            if (ollamaResponse == null)
                return Result<string>.Failure(LanguageModelErrors.EmptyResponse);

            return Result<string>.Success(ollamaResponse.Response);
        }
    }
}
