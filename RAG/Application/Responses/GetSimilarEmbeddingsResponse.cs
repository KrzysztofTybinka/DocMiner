

namespace Application.Responses
{
    public class GetSimilarEmbeddingsResponse
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public string Source { get; set; }

        public double Distance { get; set; }
    }
}
