using Domain.Abstractions;

namespace Domain.Embedings
{
    public class Embedding
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public float[] TextEmbedding {  get; set; }

        private Embedding(string text, float[] textEmbedding)
        {
            Id = Guid.NewGuid();
            Text = text;
            TextEmbedding = textEmbedding;
        }

        public static Result<Embedding> Create(string text, float[] textEmbedding)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Result<Embedding>.Failure(EmbeddingErrors.TextEmpty);
            }
            if (textEmbedding == null || textEmbedding.Length == 0)
            {
                return Result<Embedding>.Failure(EmbeddingErrors.EmbedingEmpty);
            }

            var embedding = new Embedding(text, textEmbedding);
            return Result< Embedding>.Success(embedding);
        }
    }
}
