

using Application.Abstractions;
using Application.Queries.GetSimilarEmbeddings;
using Application.Responses;
using Domain.Abstractions;
using Domain.Embedings;
using Infrastructure.Abstractions;
using Moq;

namespace Tests.Application.UnitTests
{
    public class GetSimilarEmbeddingsHandlerTests
    {
        private readonly Mock<IGetSimilarEmbeddingsQueryHandlerFactory> _queryHandlerFactoryMock;
        private readonly Mock<IGetSimilarEmbeddingsQueryHandler> _queryHandlerMock;
        private readonly Mock<IEmbeddingGeneratorFactory> _embeddingGeneratorFactoryMock;
        private readonly Mock<IEmbeddingGenerator> _embeddingGeneratorMock;
        // The EmbeddingsRepositoryFactory is not used in this handler so we can ignore it.
        private readonly GetSimilarEmbeddingsHandler _handler;

        public GetSimilarEmbeddingsHandlerTests()
        {
            _queryHandlerFactoryMock = new Mock<IGetSimilarEmbeddingsQueryHandlerFactory>();
            _queryHandlerMock = new Mock<IGetSimilarEmbeddingsQueryHandler>();
            _embeddingGeneratorFactoryMock = new Mock<IEmbeddingGeneratorFactory>();
            _embeddingGeneratorMock = new Mock<IEmbeddingGenerator>();

            _handler = new GetSimilarEmbeddingsHandler();
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenQueryHandlerFactoryFails()
        {
            // Arrange: Setup the factory to return a failure.
            _queryHandlerFactoryMock
                .Setup(f => f.CreateHandlerAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<IGetSimilarEmbeddingsQueryHandler>.Failure(MockErrors.TestError));

            var query = new GetSimilarEmbeddingsQuery
            {
                CollectionName = "TestCollection",
                Prompt = "Test prompt",
                Nresults = 5,
                Source = "TestSource",
                MaxDistance = 0.5,
                QueryHandlerFactory = _queryHandlerFactoryMock.Object,
                EmbeddingGeneratorFactory = _embeddingGeneratorFactoryMock.Object,
                // EmbeddingsRepositoryFactory is not used in this handler.
                EmbeddingsRepositoryFactory = Mock.Of<IEmbeddingRepositoryFactory>()
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(MockErrors.TestError, result.Error);
            // Ensure that the embedding generator is never created.
            _embeddingGeneratorFactoryMock.Verify(e => e.CreateEmbeddingGenerator(), Times.Never);
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenEmbeddingGenerationFails()
        {
            // Arrange: Factory returns a valid query handler.
            _queryHandlerFactoryMock
                .Setup(f => f.CreateHandlerAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<IGetSimilarEmbeddingsQueryHandler>.Success(_queryHandlerMock.Object));

            // Arrange: Embedding generator factory returns our mocked generator.
            _embeddingGeneratorFactoryMock
                .Setup(f => f.CreateEmbeddingGenerator())
                .Returns(_embeddingGeneratorMock.Object);

            // Arrange: The embedding generator fails to generate an embedding.
            _embeddingGeneratorMock
                .Setup(e => e.GenerateEmbeddingAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<Embedding>.Failure(MockErrors.TestError));

            var query = new GetSimilarEmbeddingsQuery
            {
                CollectionName = "TestCollection",
                Prompt = "Test prompt",
                Nresults = 5,
                Source = "TestSource",
                MaxDistance = 0.5,
                QueryHandlerFactory = _queryHandlerFactoryMock.Object,
                EmbeddingGeneratorFactory = _embeddingGeneratorFactoryMock.Object,
                EmbeddingsRepositoryFactory = Mock.Of<IEmbeddingRepositoryFactory>()
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(MockErrors.TestError, result.Error);
            // Ensure that the query handler's Handle method is not called.
            _queryHandlerMock.Verify(q => q.Handle(It.IsAny<Embedding>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenQueryHandlerFails()
        {
            // Arrange: Factory returns a valid query handler.
            _queryHandlerFactoryMock
                .Setup(f => f.CreateHandlerAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<IGetSimilarEmbeddingsQueryHandler>.Success(_queryHandlerMock.Object));

            // Arrange: Embedding generator factory returns our mocked generator.
            _embeddingGeneratorFactoryMock
                .Setup(f => f.CreateEmbeddingGenerator())
                .Returns(_embeddingGeneratorMock.Object);

            // Create a dummy embedding to pass to the query handler.
            var dummyEmbedding = Embedding.Create(Guid.NewGuid(), "Example text", [1, 2, 3]);

            // Arrange: Embedding generation succeeds.
            _embeddingGeneratorMock
                .Setup(e => e.GenerateEmbeddingAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<Embedding>.Success(dummyEmbedding.Data));

            // Arrange: The query handler returns a failure.
            _queryHandlerMock
                .Setup(q => q.Handle(dummyEmbedding.Data, It.IsAny<string>(), It.IsAny<double>(), It.IsAny<int>()))
                .ReturnsAsync(Result<List<GetSimilarEmbeddingsResponse>>.Failure(MockErrors.TestError));

            var query = new GetSimilarEmbeddingsQuery
            {
                CollectionName = "TestCollection",
                Prompt = "Test prompt",
                Nresults = 5,
                Source = "TestSource",
                MaxDistance = 0.5,
                QueryHandlerFactory = _queryHandlerFactoryMock.Object,
                EmbeddingGeneratorFactory = _embeddingGeneratorFactoryMock.Object,
                EmbeddingsRepositoryFactory = Mock.Of<IEmbeddingRepositoryFactory>()
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(MockErrors.TestError, result.Error);
            _queryHandlerMock.Verify(q => q.Handle(dummyEmbedding.Data, query.Source, query.MaxDistance, query.Nresults), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsSuccess_WhenEverythingSucceeds()
        {
            // Arrange: Factory returns a valid query handler.
            _queryHandlerFactoryMock
                .Setup(f => f.CreateHandlerAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<IGetSimilarEmbeddingsQueryHandler>.Success(_queryHandlerMock.Object));

            // Arrange: Embedding generator factory returns our mocked generator.
            _embeddingGeneratorFactoryMock
                .Setup(f => f.CreateEmbeddingGenerator())
                .Returns(_embeddingGeneratorMock.Object);

            // Create a dummy embedding to pass to the query handler.
            var dummyEmbedding = Embedding.Create(Guid.NewGuid(), "Example text", [1, 2, 3]);

            // Arrange: Embedding generation succeeds.
            _embeddingGeneratorMock
                .Setup(e => e.GenerateEmbeddingAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<Embedding>.Success(dummyEmbedding.Data));

            // Arrange: The query handler returns a successful result.
            var expectedResponses = new List<GetSimilarEmbeddingsResponse>
            {
                new GetSimilarEmbeddingsResponse { Id = Guid.NewGuid(), Text = "Response1", Source = "TestSource", Distance = 0.1 },
                new GetSimilarEmbeddingsResponse { Id = Guid.NewGuid(), Text = "Response2", Source = "TestSource", Distance = 0.2 }
            };

            _queryHandlerMock
                .Setup(q => q.Handle(dummyEmbedding.Data, It.IsAny<string>(), It.IsAny<double>(), It.IsAny<int>()))
                .ReturnsAsync(Result<List<GetSimilarEmbeddingsResponse>>.Success(expectedResponses));

            var query = new GetSimilarEmbeddingsQuery
            {
                CollectionName = "TestCollection",
                Prompt = "Test prompt",
                Nresults = 5,
                Source = "TestSource",
                MaxDistance = 0.5,
                QueryHandlerFactory = _queryHandlerFactoryMock.Object,
                EmbeddingGeneratorFactory = _embeddingGeneratorFactoryMock.Object,
                EmbeddingsRepositoryFactory = Mock.Of<IEmbeddingRepositoryFactory>()
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedResponses, result.Data);
            _queryHandlerMock.Verify(q => q.Handle(dummyEmbedding.Data, query.Source, query.MaxDistance, query.Nresults), Times.Once);
        }
    }
}
