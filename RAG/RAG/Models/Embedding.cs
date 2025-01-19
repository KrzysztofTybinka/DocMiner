namespace RAG.Models
{
    public class Embedding
    {
        public int Id { get; set; }
        public List<float> EmbeddingVector { get; set; }
        public string Chunk { get; set; }
        public Metadata Metadata { get; set; }
    }

    public class Metadata
    {
        public string DocumentId { get; set; }
        public string ChunkId { get; set; }
        public string DocumentName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
