namespace RAG.Models
{
    public class DocumentChunk
    {
        public string Id { get; set; }
        public IEnumerable<float> EmbeddingVector { get; set; }
        public string Chunk { get; set; }
        public string DocumentName { get; set; }
    }
}
