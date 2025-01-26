using ChromaDB.Client;
using ChromaDB.Client.Models;
using Microsoft.AspNetCore.Http.Connections;
using RAG.BLL.Chunking;
using RAG.Common;
using RAG.Models;
using System.Collections.Generic;
using System.Linq;

namespace RAG.Repository
{
    public class EmbeddingsRepository : IEmbeddingsRepository
    {
        private readonly ChromaConfigurationOptions _options;
        private readonly HttpClient _httpClient;

        public EmbeddingsRepository(string chromaUrl, HttpClient httpClient)
        {
            _options = new ChromaConfigurationOptions(chromaUrl);
            _httpClient = httpClient;
        }

        public async Task<List<List<ChromaCollectionQueryEntry>>>
            QueryCollection(ChromaCollection collection, int nResults,
            IEnumerable<DocumentChunk> embeddings, ChromaWhereOperator? where = null, 
            ChromaWhereDocumentOperator? whereDocument = null, 
            ChromaQueryInclude? include = null)
        {
            var client = new ChromaCollectionClient(collection, _options, _httpClient);

            List<ReadOnlyMemory<float>> queryEmbeddings = embeddings
                .Select(e => new ReadOnlyMemory<float>(e.EmbeddingVector.ToArray()))
                .ToList();

            return await client.Query(queryEmbeddings, nResults, 
                where, whereDocument, include);
        }

        public async Task UploadDocument(IEnumerable<DocumentChunk> chunks, ChromaCollection collection, string fileName)
        {
            var client = new ChromaCollectionClient(collection, _options, _httpClient);

            List<ReadOnlyMemory<float>> embeddings = chunks
                .Select(chunk => new ReadOnlyMemory<float>(chunk.EmbeddingVector.ToArray()))
                .ToList();

            List<string> documents = chunks
                .Select(chunk => chunk.Chunk)
                .ToList();

            int index = 1;
            List<string> ids = chunks
                .Select(chunk => fileName + "_" + index++)
                .ToList();

            List<Dictionary<string, object>> metadatas = chunks
                .Select(chunk => chunk.Metadata)
                .ToList();

            await client.Add(ids, embeddings, metadatas, documents);
        }
    }
}
