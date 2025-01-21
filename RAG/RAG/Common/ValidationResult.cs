namespace RAG.Common
{
    public class ValidationResult
    {
        public List<string> Errors { get; set; } = [];
        public bool IsValid { get; set; }
    }
}
