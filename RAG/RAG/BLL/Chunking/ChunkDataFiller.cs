using RAG.Models;
using System.Runtime.CompilerServices;

namespace RAG.BLL.Chunking
{
    public class ChunkDataFiller
    {
        public static List<DocumentChunk> Fill(List<DocumentChunk> chunks, string fileName)
        {
            for (int i = 0; i < chunks.Count; i++)
            {
                chunks[i].Id = fileName + "_" + (i + 1).ToString();
                chunks[i].Metadata.Add("file name", fileName);
            }
            return chunks;
        }
    }
}
