

using Domain.Abstractions;
using Domain.Message;
using Infrastructure.Configuration;
using Infrastructure.Responses.LanguageModels;
using Infrastructure.Services.LanguageModelServices;
using Infrastructure.Services.LanguageModelServices.Parameters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace Infrastructure.Services.AnswearGeneratorServices
{
    public class OpenAILanguageModelService : IAnswearGenerator
    {
        private readonly LanguageModelSettings _settings;
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, object>? _parameters;

        public OpenAILanguageModelService(IOptions<LanguageModelSettings> embeddingModelSettings,
            IHttpClientFactory httpClientFactory,
            Dictionary<string, object>? parameters)
        {
            _settings = embeddingModelSettings.Value;
            _httpClient = httpClientFactory.CreateClient("LanguageModelClient");
            _parameters = parameters;
        }

        public async Task<Result<string>> GenerateAnswearAsync(Message message)
        {
            var openAiParams = new OpenAiParameters(_parameters);

            var requestBody = new
            {
                model = _settings.ModelName,
                messages = new[]
                {
                    new { role = "user", content = message.Content }
                },
                temperature = openAiParams.Temperature,
                max_tokens = openAiParams.MaxTokens,
                top_p = openAiParams.TopP,
                frequency_penalty = openAiParams.FrequencyPenalty,
                presence_penalty = openAiParams.PresencePenalty,
                n = openAiParams.N,
                stream = openAiParams.Stream,
                stop = openAiParams.Stop
            };

            var jsonPayload = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.Token}");

            var response = await _httpClient.PostAsync(_settings.Url, content);

            if (!response.IsSuccessStatusCode)
            {
                return Result<string>.Failure(
                    LanguageModelErrors.CouldntProcess(_settings.ModelName,
                        await response.Content.ReadAsStringAsync()));
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var openAiResponse = JsonConvert.DeserializeObject<OpenAiLanguageModelResponse>(jsonResponse);

            if (openAiResponse == null || openAiResponse.Choices == null || !openAiResponse.Choices.Any())
                return Result<string>.Failure(LanguageModelErrors.EmptyResponse);

            return Result<string>.Success(openAiResponse.Choices[0].Message.Content);
        }
    }
}
