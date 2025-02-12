using Newtonsoft.Json;

namespace Infrastructure.Responses.EmbeddingModels
{
    public class OllamaCreateEmbeddingsResponse
    {
        [JsonProperty("data")]
        public List<EmbeddingData> Data { get; set; }
    }

    public class EmbeddingData
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("embedding")]
        public List<float> Embedding { get; set; }
    }
}
