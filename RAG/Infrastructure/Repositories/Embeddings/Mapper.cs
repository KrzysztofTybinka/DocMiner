
using Domain.Embedings;

namespace Infrastructure.Repositories.DocumentRepository
{
    public static class Mapper
    {
        public static ChromaDocumentAddRequest EmbeddingsToChromaDbAddRequest(this List<Embedding> document)
        {
            var request = new ChromaDocumentAddRequest();

            request.Vectors = document
                .Select(e => new ReadOnlyMemory<float>(e.TextEmbedding.ToArray()))
                .ToList();

            request.TextContent = document
                .Select(e => e.Text)
                .ToList();

            request.Ids = document
                .Select(e => e.Id.ToString())
                .ToList();

            request.Metadatas = document
                .Select(e =>
                {
                    var source = e.Details?.Source;
                    return !string.IsNullOrEmpty(source)
                        ? new Dictionary<string, object> { { "source", source } }
                        : new Dictionary<string, object>(); // Return an empty dictionary if null
                })
                .ToList();


            return request;
        }
    }

    public class ChromaDocumentAddRequest
    {
        public List<ReadOnlyMemory<float>> Vectors { get; set; }
        public List<string> TextContent { get; set; }
        public List<string> Ids { get; set; }
        public List<Dictionary<string, object>> Metadatas { get; set; }
    }
}
