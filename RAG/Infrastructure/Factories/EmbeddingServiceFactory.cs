using Application.Abstractions;
using Domain.Embedings;
using Infrastructure.Configuration;
using Infrastructure.Services.EmbeddingService;
using Microsoft.Extensions.Options;

namespace Infrastructure.Factories
{
    public class EmbeddingServiceFactory : IEmbeddingGeneratorFactory
    {
        private readonly IOptions<EmbeddingModelSettings> _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public EmbeddingServiceFactory(IOptions<EmbeddingModelSettings> settings,
            IHttpClientFactory httpClientFactory)
        {
            _settings = settings;
            _httpClientFactory = httpClientFactory;
        }

        public IEmbeddingGenerator CreateEmbeddingGenerator()
        {
            return _settings.Value.ProviderName switch
            {
                "OpenAI" => new OpenAIEmbeddingService(_settings, _httpClientFactory),
                "Ollama" => new OllamaEmbeddingService(_settings, _httpClientFactory),
                _ => throw new NotSupportedException($"Provider '{_settings.Value.ProviderName}' is not supported.")
            };
        }
    }
}
