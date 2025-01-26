using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using RAG.Models;

namespace RAG.Services.Embedding
{
    public class EmbeddingServiceFactory
    {
        private readonly IOptions<EmbeddingModelSettings> _embeddingModelSettings;

        public EmbeddingServiceFactory(IOptions<EmbeddingModelSettings> embeddingModelSettings)
        {
            _embeddingModelSettings = embeddingModelSettings; ;
        }

        public EmbeddingService CreateEmbeddingModel()
        {
            switch (_embeddingModelSettings.Value.ProviderName)
            {
                case "OpenAI":
                    return new OpenAIEmbeddingService(_embeddingModelSettings);
                case "Ollama":
                    return new OllamaEmbeddingService(_embeddingModelSettings);

                default:
                    throw new ArgumentException($"Model '{_embeddingModelSettings.Value.ProviderName}' is not supported.");
            }
        }
    }
}
