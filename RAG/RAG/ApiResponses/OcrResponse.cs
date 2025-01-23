using System.Text.Json.Serialization;

namespace RAG.ApiResponses
{
    public class OcrResponse
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
