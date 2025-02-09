using Domain.Abstractions;

namespace Domain.Embedings
{
    public class Embedding
    {
        public Guid Id { get; private set; }

        public string Text { get; private set; }

        public float[] TextEmbedding {  get; private set; }

        public EmbeddingDetails? Details { get; set; }

        private Embedding(Guid id, string text, float[] textEmbedding, EmbeddingDetails? details)
        {
            Id = id;
            Text = text;
            TextEmbedding = textEmbedding;
            Details = details;
        }

        public static Result<Embedding> Create(Guid id, string text, float[] textEmbedding, EmbeddingDetails? details = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Result<Embedding>.Failure(EmbeddingErrors.TextEmpty);
            }
            if (textEmbedding == null || textEmbedding.Length == 0)
            {
                return Result<Embedding>.Failure(EmbeddingErrors.EmbedingEmpty);
            }

            var embedding = new Embedding(id, text, textEmbedding, details);
            return Result< Embedding>.Success(embedding);
        }
    }
}
