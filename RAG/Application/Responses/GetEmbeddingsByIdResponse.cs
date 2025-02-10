
namespace Application.Responses
{
    public class GetEmbeddingsByIdResponse
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public string Source { get; set; }
    }
}
