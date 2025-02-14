

using Domain.Embedings;

namespace Tests.Domain.Tests
{
    public class EmbeddingTest
    {
        private static readonly Guid TestId = Guid.NewGuid();
        private const string ValidText = "sample text";
        private static readonly float[] ValidEmbedding = [0.1f, 0.2f, 0.3f];
        private static readonly EmbeddingDetails TestDetails = new() { Source = "test-source" };

        [Fact]
        public void Create_ValidParameters_ReturnsEmbedding()
        {
            // Act
            var result = Embedding.Create(TestId, ValidText, ValidEmbedding);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(TestId, result.Data.Id);
            Assert.Equal(ValidText, result.Data.Text);
            Assert.Equal(ValidEmbedding, result.Data.TextEmbedding);
            Assert.Null(result.Data.Details);
        }

        [Fact]
        public void Create_WithDetails_IncludesDetails()
        {
            // Act
            var result = Embedding.Create(TestId, ValidText, ValidEmbedding, TestDetails);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(TestDetails, result.Data.Details);
            Assert.Equal("test-source", result.Data.Details?.Source);
        }

        [Fact]
        public void Create_EmptyText_ReturnsTextEmptyError()
        {
            // Act
            var result = Embedding.Create(TestId, "", ValidEmbedding);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(EmbeddingErrors.TextEmpty.Code, result.Error.Code);
            Assert.Contains("provide text", result.Error.Description);
        }

        [Fact]
        public void Create_WhitespaceText_ReturnsTextEmptyError()
        {
            // Act
            var result = Embedding.Create(TestId, "   ", ValidEmbedding);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(EmbeddingErrors.TextEmpty.Code, result.Error.Code);
        }

        [Fact]
        public void Create_NullEmbedding_ReturnsEmbeddingEmptyError()
        {
            // Act
            var result = Embedding.Create(TestId, ValidText, null!);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(EmbeddingErrors.EmbedingEmpty.Code, result.Error.Code);
            Assert.Contains("Vector embedding", result.Error.Description);
        }

        [Fact]
        public void Create_EmptyEmbeddingArray_ReturnsEmbeddingEmptyError()
        {
            // Act
            var result = Embedding.Create(TestId, ValidText, Array.Empty<float>());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(EmbeddingErrors.EmbedingEmpty.Code, result.Error.Code);
        }

        [Fact]
        public void Create_MinimumValidParameters_Succeeds()
        {
            // Arrange
            var minText = "a";
            var minEmbedding = new float[] { 0f };

            // Act
            var result = Embedding.Create(TestId, minText, minEmbedding);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(minText, result.Data.Text);
            Assert.Equal(minEmbedding, result.Data.TextEmbedding);
        }

        [Fact]
        public void Create_WithEmptyGuid_Allowed()
        {
            // Arrange
            var emptyId = Guid.Empty;

            // Act
            var result = Embedding.Create(emptyId, ValidText, ValidEmbedding);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(Guid.Empty, result.Data.Id);
        }
    }
}
