

using Application.Abstractions;
using Application.Queries;
using Application.Responses;
using Domain.Abstractions;
using Infrastructure.Abstractions;
using Moq;
using RAG.Requests;

namespace Tests.Application.UnitTests
{
    public class GetEmbeddingsByIdHandlerTests
    {
        private readonly Mock<IGetEmbeddingsByIdQueryHandlerFactory> _factoryMock;
        private readonly Mock<IGetEmbeddingsByIdQueryHandler> _queryHandlerMock;
        private readonly GetEmbeddingsByIdHandler _handler;

        public GetEmbeddingsByIdHandlerTests()
        {
            _factoryMock = new Mock<IGetEmbeddingsByIdQueryHandlerFactory>();
            _queryHandlerMock = new Mock<IGetEmbeddingsByIdQueryHandler>();

            _handler = new GetEmbeddingsByIdHandler();
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenCreateHandlerFails()
        {
            // Arrange
            var expectedError = MockErrors.TestError;
            _factoryMock
                .Setup(f => f.CreateHandlerAsync(It.IsAny<string>()))
                .ReturnsAsync(Result<IGetEmbeddingsByIdQueryHandler>.Failure(MockErrors.TestError));

            var query = new GetEmbeddingsQuery
            {
                CollectionName = "TestCollection",
                QueryhandlerFactory = _factoryMock.Object,
                Ids = new[] { Guid.NewGuid().ToString() },
                Source = "TestSource"
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Error);

            // Verify that the query handler's Handle method was never called.
            _queryHandlerMock.Verify(q => q.Handle(It.IsAny<List<Guid>?>(), It.IsAny<string?>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ReturnsFailure_WhenQueryHandlerFails()
        {
            // Arrange
            var factoryResult = Result<IGetEmbeddingsByIdQueryHandler>.Success(_queryHandlerMock.Object);
            _factoryMock
                .Setup(f => f.CreateHandlerAsync(It.IsAny<string>()))
                .ReturnsAsync(factoryResult);

            var expectedError = MockErrors.TestError;
            _queryHandlerMock
                .Setup(q => q.Handle(It.IsAny<List<Guid>?>(), It.IsAny<string?>()))
                .ReturnsAsync(Result<List<GetEmbeddingsByIdResponse>>.Failure(MockErrors.TestError));

            var query = new GetEmbeddingsQuery
            {
                CollectionName = "TestCollection",
                QueryhandlerFactory = _factoryMock.Object,
                // Providing one valid Guid string.
                Ids = [Guid.NewGuid().ToString()],
                Source = "TestSource"
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Error);

            _factoryMock.Verify(f => f.CreateHandlerAsync(query.CollectionName), Times.Once);
            _queryHandlerMock.Verify(q => q.Handle(
                It.IsAny<List<Guid>?>(),
                query.Source),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsSuccess_WhenQueryHandlerSucceeds_WithValidIds()
        {
            // Arrange
            var factoryResult = Result<IGetEmbeddingsByIdQueryHandler>.Success(_queryHandlerMock.Object);
            _factoryMock
                .Setup(f => f.CreateHandlerAsync(It.IsAny<string>()))
                .ReturnsAsync(factoryResult);

            var responseList = new List<GetEmbeddingsByIdResponse>
            {
                new GetEmbeddingsByIdResponse { Id = Guid.NewGuid(), Text = "Text1", Source = "TestSource" },
                new GetEmbeddingsByIdResponse { Id = Guid.NewGuid(), Text = "Text2", Source = "TestSource" }
            };

            _queryHandlerMock
                .Setup(q => q.Handle(It.IsAny<List<Guid>?>(), It.IsAny<string?>()))
                .ReturnsAsync(Result<List<GetEmbeddingsByIdResponse>>.Success(responseList));

            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var query = new GetEmbeddingsQuery
            {
                CollectionName = "TestCollection",
                QueryhandlerFactory = _factoryMock.Object,
                Ids = new[] { id1.ToString(), id2.ToString() },
                Source = "TestSource"
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(responseList, result.Data);

            _factoryMock.Verify(f => f.CreateHandlerAsync(query.CollectionName), Times.Once);
            _queryHandlerMock.Verify(q => q.Handle(
                It.Is<List<Guid>>(guids => guids != null
                    && guids.Count == 2
                    && guids.Contains(id1)
                    && guids.Contains(id2)),
                query.Source),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsSuccess_WhenIdsIsNullOrEmpty()
        {
            // Arrange
            var factoryResult = Result<IGetEmbeddingsByIdQueryHandler>.Success(_queryHandlerMock.Object);
            _factoryMock
                .Setup(f => f.CreateHandlerAsync(It.IsAny<string>()))
                .ReturnsAsync(factoryResult);

            var responseList = new List<GetEmbeddingsByIdResponse>
            {
                new GetEmbeddingsByIdResponse { Id = Guid.NewGuid(), Text = "Text1", Source = "TestSource" }
            };

            _queryHandlerMock
                .Setup(q => q.Handle(It.IsAny<List<Guid>?>(), It.IsAny<string?>()))
                .ReturnsAsync(Result<List<GetEmbeddingsByIdResponse>>.Success(responseList));

            // Test with null IDs.
            var queryWithNullIds = new GetEmbeddingsQuery
            {
                CollectionName = "TestCollection",
                QueryhandlerFactory = _factoryMock.Object,
                Ids = null,
                Source = "TestSource"
            };

            var resultNullIds = await _handler.Handle(queryWithNullIds, CancellationToken.None);

            Assert.True(resultNullIds.IsSuccess);
            Assert.Equal(responseList, resultNullIds.Data);
            _queryHandlerMock.Verify(q => q.Handle(null, queryWithNullIds.Source), Times.Once);

            // Reset mock invocation count for the next scenario.
            _queryHandlerMock.Invocations.Clear();

            // Test with empty IDs array.
            var queryWithEmptyIds = new GetEmbeddingsQuery
            {
                CollectionName = "TestCollection",
                QueryhandlerFactory = _factoryMock.Object,
                Ids = new string[0],
                Source = "TestSource"
            };

            var resultEmptyIds = await _handler.Handle(queryWithEmptyIds, CancellationToken.None);

            Assert.True(resultEmptyIds.IsSuccess);
            Assert.Equal(responseList, resultEmptyIds.Data);
            _queryHandlerMock.Verify(q => q.Handle(null, queryWithEmptyIds.Source), Times.Once);
        }
    }
}
