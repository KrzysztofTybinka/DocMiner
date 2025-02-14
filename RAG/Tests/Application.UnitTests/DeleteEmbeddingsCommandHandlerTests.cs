

using Application.Commands.DeleteEmbeddings;
using Domain.Abstractions;
using Domain.Embedings;
using Infrastructure.Abstractions;
using Moq;

namespace Tests.Application.UnitTests
{
    public class DeleteEmbeddingsCommandHandlerTests
    {
        private readonly Mock<IEmbeddingRepositoryFactory> _repoFactoryMock = new();
        private readonly Mock<IEmbeddingRepository> _repoMock = new();
        private readonly DeleteEmbeddingsCommandHandler _handler;

        public DeleteEmbeddingsCommandHandlerTests()
        {
            _repoFactoryMock.Setup(f => f.CreateRepositoryAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<IEmbeddingRepository>.Success(_repoMock.Object));

            // Default setup for successful deletion
            _repoMock.Setup(r => r.DeleteEmbeddingsAsync(It.IsAny<List<Guid>>()))
                .ReturnsAsync(Result.Success());

            _handler = new DeleteEmbeddingsCommandHandler();
        }


        [Fact]
        public async Task Handle_ValidRequest_DeletesEmbeddings()
        {
            // Arrange
            var testIds = new[] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() };
            var command = CreateCommand(ids: testIds);

            // Setup the repository mock to return success
            _repoMock.Setup(r => r.DeleteEmbeddingsAsync(It.IsAny<List<Guid>>()))
                .ReturnsAsync(Result.Success()); // <-- ADD THIS

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            _repoMock.Verify(r => r.DeleteEmbeddingsAsync(
                It.Is<List<Guid>>(ids =>
                    ids.Count == testIds.Length &&
                    testIds.All(id => ids.Contains(Guid.Parse(id)))
                )
            ), Times.Once);
        }

        [Fact]
        public async Task Handle_RepositoryCreationFails_ReturnsError()
        {
            // Arrange
            var expectedError = Error.Failure("TestError", "Repository creation failed");
            _repoFactoryMock.Setup(f => f.CreateRepositoryAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<IEmbeddingRepository>.Failure(expectedError));

            var command = CreateCommand();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError.Code, result.Error.Code);
        }

        [Fact]
        public async Task Handle_DeleteOperationFails_PropagatesError()
        {
            // Arrange
            var expectedError = Error.Failure("TestError", "Delete failed");
            _repoMock.Setup(r => r.DeleteEmbeddingsAsync(It.IsAny<List<Guid>>()))
                .ReturnsAsync(Result.Failure(expectedError));

            var command = CreateCommand();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError.Code, result.Error.Code);
        }

        [Fact]
        public async Task Handle_EmptyIds_DeletesNothing()
        {
            // Arrange
            var command = CreateCommand(ids: Array.Empty<string>());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repoMock.Verify(r => r.DeleteEmbeddingsAsync(
                It.Is<List<Guid>>(ids => !ids.Any())),
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_InvalidGuidFormat_ThrowsException()
        {
            // Arrange
            var command = CreateCommand(ids: ["invalid-guid-format"]);

            // Act & Assert
            await Assert.ThrowsAsync<FormatException>(() =>
                _handler.Handle(command, CancellationToken.None)
            );
        }

        private DeleteEmbeddingsCommand CreateCommand(string[]? ids = null) => new()
        {
            EmbeddingsRepositoryFactory = _repoFactoryMock.Object,
            CollectionName = "test-collection",
            Ids = ids ?? [Guid.NewGuid().ToString()]
        };
    }
}

