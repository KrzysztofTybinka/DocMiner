namespace RAG.Models
{
    public class DocumentChunk
    {
        public IEnumerable<float> EmbeddingVector { get; set; }
        public string Chunk { get; set; }
    }
}
