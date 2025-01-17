using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using RAG.Models;

namespace RAG.Services.Embedding
{
    public class EmbeddingFactory
    {
        private readonly IOptions<EmbeddingModelSettings> _embeddingModelSettings;

        public EmbeddingFactory(IOptions<EmbeddingModelSettings> embeddingModelSettings)
        {
            _embeddingModelSettings = embeddingModelSettings; ;
        }

        public IEmbedding CreateEmbeddingModel()
        {
            switch (_embeddingModelSettings.Value.ProviderName)
            {
                case "OpenAI":
                    return new OpenAIEmbeddingService(_embeddingModelSettings);

                default:
                    throw new ArgumentException($"Model '{_embeddingModelSettings.Value.ProviderName}' is not supported.");
            }
        }
    }
}
