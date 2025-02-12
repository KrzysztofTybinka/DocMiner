

namespace Infrastructure.Services.LanguageModelServices.Parameters
{
    public class OpenAiParameters
    {
        public double Temperature { get; set; } = 0.7;
        public int MaxTokens { get; set; } = 150;
        public double TopP { get; set; } = 1.0;
        public double FrequencyPenalty { get; set; } = 0.0;
        public double PresencePenalty { get; set; } = 0.0;
        public int N { get; set; } = 1;
        public bool Stream { get; set; } = false;
        public string[]? Stop { get; set; } = null;

        public OpenAiParameters(Dictionary<string, object>? parameters)
        {
            parameters ??= new Dictionary<string, object>();

            Temperature = parameters.TryGetValue("temperature", out var temp) && temp is double d ? d : 0.7;
            MaxTokens = parameters.TryGetValue("max_tokens", out var max) && max is int i ? i : 150;
            TopP = parameters.TryGetValue("top_p", out var top) && top is double tp ? tp : 1.0;
            FrequencyPenalty = parameters.TryGetValue("frequency_penalty", out var freq) && freq is double fp ? fp : 0.0;
            PresencePenalty = parameters.TryGetValue("presence_penalty", out var pres) && pres is double pr ? pr : 0.0;
            N = parameters.TryGetValue("n", out var nVal) && nVal is int nInt ? nInt : 1;
            Stream = parameters.TryGetValue("stream", out var streamVal) && streamVal is bool s ? s : false;

            if (parameters.TryGetValue("stop", out var stopVal))
            {
                // Accept a single string or an array of strings
                if (stopVal is string[] stops)
                {
                    Stop = stops;
                }
                else if (stopVal is string singleStop)
                {
                    Stop = [singleStop];
                }
            }
        }
    }
}
