using Domain.ProcessedDocument;


namespace Tests.Domain.Tests
{
    public class DocumentChunkerTests
    {
        [Fact]
        public void ChunkContent_ZeroTokenAmount_ReturnsError()
        {
            var result = DocumentChunker.ChunkContent("valid content", 0, [".", "!"]);
            Assert.True(!result.IsSuccess);
            Assert.Equal("DocumentChunker.InvalidTokenAmount", result.Error.Code);
        }

        [Fact]
        public void ChunkContent_ExactTokenAmountWithDelimiter_CorrectChunk()
        {
            var result = DocumentChunker.ChunkContent("a b c.", 3, [".", "!"]);
            Assert.Equal(["a b c."], result.Data);
        }

        [Fact]
        public void ChunkContent_ContentShorterThanTokenAmount_ReturnsEmpty()
        {
            var result = DocumentChunker.ChunkContent("a b c", 4, [".", "!"]);
            Assert.Empty(result.Data);
        }

        [Fact]
        public void ChunkContent_MultipleDelimiters_CorrectChunks()
        {
            var content = "First sentence. Second sentence! Third? Fourth";
            var result = DocumentChunker.ChunkContent(content, 2, [".", "!", "?"]);

            Assert.Equal(3, result.Data.Count);
            Assert.Equal("First sentence.", result.Data[0]);
            Assert.Equal("Second sentence!", result.Data[1]);
            Assert.Equal("Third? Fourth", result.Data[2]);
        }

        [Fact]
        public void ChunkContent_NoDelimiters_UsesExactTokenAmount()
        {
            var content = "a b c d e f g h i j";
            var result = DocumentChunker.ChunkContent(content, 3, []);

            Assert.Equal(3, result.Data.Count);
            Assert.Equal("a b c", result.Data[0]);
            Assert.Equal("d e f", result.Data[1]);
            Assert.Equal("g h i", result.Data[2]);
        }

        [Fact]
        public void ChunkContent_RemainingTokens_CreatesFinalChunk()
        {
            var content = "a b c d e f g h i j k";
            var result = DocumentChunker.ChunkContent(content, 3, []);

            Assert.Equal(3, result.Data.Count);
            Assert.Equal("a b c", result.Data[0]);
            Assert.Equal("d e f", result.Data[1]);
            Assert.Equal("g h i", result.Data[2]); // Last 2 tokens (j k) are < 3, so omitted
        }
    }
}
