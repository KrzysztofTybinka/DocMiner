using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using RAG.Models;

namespace RAG.Services.Embedding
{
    public class EmbeddingFactory
    {
        private readonly EmbeddingModelSettings _embeddingModelSettings;

        public EmbeddingFactory(EmbeddingModelSettings embeddingModelSettings)
        {
            _embeddingModelSettings = embeddingModelSettings;
        }

        public IEmbedding CreateEmbeddingModel()
        {
            switch (_embeddingModelSettings.ProviderName)
            {
                case "OpenAI":
                    return new OpenAIEmbeddingService(_embeddingModelSettings);

                default:
                    throw new ArgumentException($"Model '{_embeddingModelSettings.ProviderName}' is not supported.");
            }
        }
    }
}
