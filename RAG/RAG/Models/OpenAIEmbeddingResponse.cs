namespace RAG.Models
{
    public class OpenAIEmbeddingResponse
    {
        public string Object { get; set; }
        public List<DataItem> Data { get; set; }
        public string Model { get; set; }
        public Usage Usage { get; set; }
    }

    public class DataItem
    {
        public string Object { get; set; }
        public int Index { get; set; }
        public List<float> Embedding { get; set; }
    }

    public class Usage
    {
        public int PromptTokens { get; set; }
        public int TotalTokens { get; set; }
    }
}
