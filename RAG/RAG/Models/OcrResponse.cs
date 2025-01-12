using System.Text.Json.Serialization;

namespace RAG.Models
{
    public class OcrResponse
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
