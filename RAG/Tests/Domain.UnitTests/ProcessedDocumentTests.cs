using Domain.ProcessedDocument;


namespace Tests.Domain.Tests
{
    public class ProcessedDocumentTests
    {
        [Fact]
        public void Create_ValidNameAndContent_ReturnsSuccess()
        {
            var result = ProcessedDocument.Create("test.txt", "content");

            Assert.True(result.IsSuccess);
            Assert.Equal("test.txt", result.Data.Name);
            Assert.Equal("content", result.Data.Content);
        }

        [Fact]
        public void Create_EmptyName_ReturnsNameEmptyError()
        {
            var result = ProcessedDocument.Create("", "content");

            Assert.True(!result.IsSuccess);
            Assert.Equal(ProcessedDocumentError.NameEmpty, result.Error);
        }

        [Fact]
        public void Create_EmptyContent_ReturnsNoContentError()
        {
            var result = ProcessedDocument.Create("test.txt", "");

            Assert.True(!result.IsSuccess);
            Assert.Equal(ProcessedDocumentError.NoContent, result.Error);
        }
    }
}

