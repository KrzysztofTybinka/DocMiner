using Application.Commands.ProcessDocument;
using Domain.Abstractions;
using Domain.ProcessedDocument;
using Moq;


namespace Tests.Application.UnitTests
{
    public class ProcessedDocumentServiceTests
    {
        private readonly Mock<IProcessedDocumentGenerator> _generatorMock = new();
        private readonly ProcessDocumentCommand _service;

        public ProcessedDocumentServiceTests()
        {
            _service = new ProcessDocumentCommand(_generatorMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidInput_ReturnsProcessedDocument()
        {
            // Arrange
            var expectedDoc = ProcessedDocument.Create("test.txt", "content").Data;
            _generatorMock.Setup(g => g.ProcessDocument(It.IsAny<byte[]>(), "test.txt"))
                .ReturnsAsync(Result<ProcessedDocument>.Success(expectedDoc));

            // Act
            var result = await _service.CreateAsync(new byte[10], "test.txt");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedDoc, result.Data);
        }

        [Fact]
        public async Task CreateChunksAsync_ProcessingFails_PropagatesError()
        {
            // Arrange
            var expectedError = Error.Failure("Test.Error", "Processing failed");
            _generatorMock.Setup(g => g.ProcessDocument(It.IsAny<byte[]>(), It.IsAny<string>()))
                .ReturnsAsync(Result<ProcessedDocument>.Failure(expectedError));

            // Act
            var result = await _service.CreateChunksAsync(new byte[10], "test.txt", 50);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError.Code, result.Error.Code);
        }

        [Fact]
        public async Task CreateChunksAsync_ValidContent_ReturnsChunks()
        {
            // Arrange
            var processedDoc = ProcessedDocument.Create("test.txt", "a. b! c? d;").Data;
            _generatorMock.Setup(g => g.ProcessDocument(It.IsAny<byte[]>(), It.IsAny<string>()))
                .ReturnsAsync(Result<ProcessedDocument>.Success(processedDoc));

            // Act
            var result = await _service.CreateChunksAsync(new byte[10], "test.txt", 1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(4, result.Data.Count);
            Assert.Equal(["a.", "b!", "c?", "d;"], result.Data);
        }
    }
}
