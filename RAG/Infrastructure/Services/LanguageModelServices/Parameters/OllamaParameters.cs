

namespace Infrastructure.Services.LanguageModelServices.Parameters
{
    public class OllamaParameters
    {
        public double? Temperature { get; set; } = 0.7;
        public int? MaxTokens { get; set; } = 100;

        public OllamaParameters(Dictionary<string, object>? parameters)
        {
            parameters ??= new Dictionary<string, object>();

            Temperature = parameters.TryGetValue("temperature", out var temperature) && temperature is double d ? d : 0.7;
            MaxTokens = parameters.TryGetValue("max_tokens", out var maxTokens) && maxTokens is int i ? i : 100;
        }
    }
}
