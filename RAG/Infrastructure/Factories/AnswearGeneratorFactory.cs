
using Application.Abstractions;
using Domain.Message;
using Infrastructure.Configuration;
using Infrastructure.Services.AnswearGeneratorServices;
using Infrastructure.Services.EmbeddingService;
using Microsoft.Extensions.Options;

namespace Infrastructure.Factories
{
    public class AnswearGeneratorFactory : IAnswearGeneratorFactory
    {
        private readonly IOptions<LanguageModelSettings> _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public AnswearGeneratorFactory(IOptions<LanguageModelSettings> settings,
            IHttpClientFactory httpClientFactory)
        {
            _settings = settings;
            _httpClientFactory = httpClientFactory;
        }

        //to do, dostasowa
        public IAnswearGenerator CreateAnswearGenerator(Dictionary<string, object>? parameters)
        {
            return _settings.Value.ProviderName switch
            {
                "OpenAI" => new OpenAILanguageModelService(_settings, _httpClientFactory, parameters),
                "Ollama" => new OllamaLanguageModelService(_settings, _httpClientFactory, parameters),
                _ => throw new NotSupportedException($"Provider '{_settings.Value.ProviderName}' is not supported.")
            };
        }
    }
}
