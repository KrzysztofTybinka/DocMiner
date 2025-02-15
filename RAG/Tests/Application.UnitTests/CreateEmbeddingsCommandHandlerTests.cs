using Application.Abstractions;
using Application.Commands.CreateEmbeddings;
using Application.Commands.ProcessDocument;
using Application.Common;
using Domain.Abstractions;
using Domain.Embedings;
using Domain.ProcessedDocument;
using Infrastructure.Abstractions;
using Moq;


namespace Tests.Application.UnitTests
{
    public class CreateEmbeddingsCommandHandlerTests
    {
        private readonly Mock<IEmbeddingGeneratorFactory> _generatorFactoryMock = new();
        private readonly Mock<IEmbeddingRepositoryFactory> _repoFactoryMock = new();
        private readonly Mock<IEmbeddingGenerator> _generatorMock = new();
        private readonly Mock<IEmbeddingRepository> _repoMock = new();
        private readonly ProcessDocumentCommand _docService;
        private readonly CreateEmbeddingsCommandHandler _handler;

        public CreateEmbeddingsCommandHandlerTests()
        {
            var docGeneratorMock = new Mock<IProcessedDocumentGenerator>();
            docGeneratorMock.Setup(g => g.ProcessDocument(It.IsAny<byte[]>(), It.IsAny<string>()))
                .ReturnsAsync(Result<ProcessedDocument>.Success(
                    ProcessedDocument.Create("test.txt", "content").Data
                ));

            _docService = new ProcessDocumentCommand(docGeneratorMock.Object);

            _repoFactoryMock.Setup(f => f.CreateRepositoryAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<IEmbeddingRepository>.Success(_repoMock.Object));

            _generatorFactoryMock.Setup(f => f.CreateEmbeddingGenerator())
                .Returns(_generatorMock.Object);

            _handler = new CreateEmbeddingsCommandHandler(
                _docService,
                _generatorFactoryMock.Object,
                _repoFactoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_RepositoryCreationFails_ReturnsError()
        {
            // Arrange
            var expectedError = Error.Failure("Repo.Error", "Creation failed");
            _repoFactoryMock.Setup(f => f.CreateRepositoryAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<IEmbeddingRepository>.Failure(expectedError));

            var command = CreateValidCommand();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError.Code, result.Error.Code);
        }

        [Fact]
        public async Task Handle_ChunkCreationFails_ReturnsError()
        {
            // Arrange
            var docServiceMock = new Mock<IProcessedDocumentGenerator>();
            docServiceMock.Setup(g => g.ProcessDocument(It.IsAny<byte[]>(), It.IsAny<string>()))
                .ReturnsAsync(Result<ProcessedDocument>.Failure(
                    ProcessedDocumentError.NoContent
                ));

            var failingDocService = new ProcessDocumentCommand(docServiceMock.Object);
            var command = CreateValidCommand();

            var handlerWithFailingDocService = new CreateEmbeddingsCommandHandler(
                failingDocService,
                _generatorFactoryMock.Object,
                _repoFactoryMock.Object
            );

            // Act
            var result = await handlerWithFailingDocService.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ProcessedDocumentError.NoContent.Code, result.Error.Code);
        }

        [Fact]
        public async Task Handle_EmbeddingGenerationFails_ReturnsError()
        {
            // Arrange
            var expectedError = Error.Failure("Embed.Error", "Generation failed");
            _generatorMock.Setup(g => g.GenerateEmbeddingsAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(Result<List<Embedding>>.Failure(expectedError));

            var command = CreateValidCommand();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError.Code, result.Error.Code);
        }

        [Fact]
        public async Task Handle_ValidFlow_SavesEmbeddingsWithMetadata()
        {
            // Arrange
            var testEmbeddings = new List<Embedding>
        {
            Embedding.Create(Guid.NewGuid(), "chunk1", [0.1f]).Data,
            Embedding.Create(Guid.NewGuid(), "chunk2", [0.2f]).Data
        };

            _generatorMock.Setup(g => g.GenerateEmbeddingsAsync(It.IsAny<List<string>>()))
                .ReturnsAsync(Result<List<Embedding>>.Success(testEmbeddings));

            _repoMock.Setup(r => r.UploadEmbeddingsAsync(It.IsAny<List<Embedding>>()))
                .ReturnsAsync(Result.Success());

            var command = CreateValidCommand();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repoMock.Verify(r => r.UploadEmbeddingsAsync(
                It.Is<List<Embedding>>(e =>
                    e.All(em => em.Details!.Source == "test")
                )),
                Times.Once
            );
        }

        private CreateEmbeddingsCommand CreateValidCommand() => new()
        {
            File = new FileData() { Content = new byte[10], FileName = "test.txt" },
            CollectionName = "test-collection",
            NumberOfTokens = 50
        };
    }
}
