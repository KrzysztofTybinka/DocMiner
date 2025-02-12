namespace Infrastructure.Responses.LanguageModels
{
    public class OllamaLanguageModelResponse
    {
        public string Model { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Response { get; set; } = string.Empty;
        public bool Done { get; set; }
        public List<int> Context { get; set; } = new();
        public long TotalDuration { get; set; }
        public long LoadDuration { get; set; }
        public int PromptEvalCount { get; set; }
        public long PromptEvalDuration { get; set; }
        public int EvalCount { get; set; }
        public long EvalDuration { get; set; }
    }

}
