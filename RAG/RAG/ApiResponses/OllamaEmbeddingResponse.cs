using Newtonsoft.Json;

namespace RAG.ApiResponses
{
    public class OllamaEmbeddingResponse
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
