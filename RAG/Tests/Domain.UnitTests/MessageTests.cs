using Domain.Abstractions;
using Domain.Message;
using Xunit;

namespace Tests.Domain.Tests
{
    public class MessageTests
    {
        private const string ValidQuestion = "What's the main theme?";
        private const string ValidData = "Sample document text";
        private const string ValidWrapper = "Data: {retrievedData}\nQuestion: {question}";

        [Fact]
        public void Create_ValidParameters_ReturnsFormattedMessage()
        {
            // Arrange
            var expectedContent = $"Data: {ValidData}\nQuestion: {ValidQuestion}";

            // Act
            var result = Message.Create(ValidQuestion, ValidData, ValidWrapper);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedContent, result.Data.Content);
        }

        [Fact]
        public void Create_EmptyQuestion_ReturnsQuestionEmptyError()
        {
            // Act
            var result = Message.Create("", ValidData, ValidWrapper);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(MessageErrors.QuestionEmpty.Code, result.Error.Code);
            Assert.Equal(MessageErrors.QuestionEmpty.Description, result.Error.Description);
        }

        [Fact]
        public void Create_WhiteSpaceQuestion_ReturnsQuestionEmptyError()
        {
            // Act
            var result = Message.Create("   ", ValidData, ValidWrapper);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(MessageErrors.QuestionEmpty.Code, result.Error.Code);
        }

        [Fact]
        public void Create_EmptyRetrievedData_ReturnsRetrievedDataEmptyError()
        {
            // Act
            var result = Message.Create(ValidQuestion, "", ValidWrapper);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(MessageErrors.RetrievedDataEmpty.Code, result.Error.Code);
            Assert.Equal(MessageErrors.RetrievedDataEmpty.Description, result.Error.Description);
        }

        [Fact]
        public void Create_EmptyPromptWrapper_ReturnsPromptWrapperEmptyError()
        {
            // Act
            var result = Message.Create(ValidQuestion, ValidData, "");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(MessageErrors.PromptWrapperEmpty.Code, result.Error.Code);
            Assert.Equal(MessageErrors.PromptWrapperEmpty.Description, result.Error.Description);
        }

        [Fact]
        public void Create_MissingRetrievedDataPlaceholder_ReturnsInvalidWrapperError()
        {
            // Arrange
            const string invalidWrapper = "Question: {question}";

            // Act
            var result = Message.Create(ValidQuestion, ValidData, invalidWrapper);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(MessageErrors.InvalidPromptWrapper("{retrievedData}").Code, result.Error.Code);
            Assert.Contains("{retrievedData}", result.Error.Description);
        }

        [Fact]
        public void Create_MissingQuestionPlaceholder_ReturnsInvalidWrapperError()
        {
            // Arrange
            const string invalidWrapper = "Data: {retrievedData}";

            // Act
            var result = Message.Create(ValidQuestion, ValidData, invalidWrapper);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(MessageErrors.InvalidPromptWrapper("{question}").Code, result.Error.Code);
            Assert.Contains("{question}", result.Error.Description);
        }

        [Fact]
        public void Create_MultipleMissingPlaceholders_ReturnsFirstMissingError()
        {
            // Arrange
            const string invalidWrapper = "No placeholders here";

            // Act
            var result = Message.Create(ValidQuestion, ValidData, invalidWrapper);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(MessageErrors.InvalidPromptWrapper("{retrievedData}").Code, result.Error.Code);
        }

        [Fact]
        public void Create_PlaceholderCaseSensitive_ReturnsInvalidWrapperError()
        {
            // Arrange
            const string invalidWrapper = "Data: {RetrievedData} Question: {Question}";

            // Act
            var result = Message.Create(ValidQuestion, ValidData, invalidWrapper);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(MessageErrors.InvalidPromptWrapper("{retrievedData}").Code, result.Error.Code);
        }

        [Fact]
        public void Create_ComplexWrapperWithMultipleReplacements_FormatsCorrectly()
        {
            // Arrange
            const string complexWrapper = "Analysis Request:\n{retrievedData}\n\nUser Query: {question}";
            var expectedContent = $"Analysis Request:\n{ValidData}\n\nUser Query: {ValidQuestion}";

            // Act
            var result = Message.Create(ValidQuestion, ValidData, complexWrapper);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedContent, result.Data.Content);
        }

        [Fact]
        public void Create_ExtraPlaceholdersInWrapper_KeepsUntouched()
        {
            // Arrange
            const string wrapperWithExtras = "Data: {retrievedData} Question: {question} Extra: {unused}";
            var expectedContent = $"Data: {ValidData} Question: {ValidQuestion} Extra: {{unused}}";

            // Act
            var result = Message.Create(ValidQuestion, ValidData, wrapperWithExtras);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedContent, result.Data.Content);
        }
    }
}