

using Application.Abstractions;
using Application.Commands.GenerateResponse;
using Domain.Abstractions;
using Domain.Message;
using Moq;

namespace Tests.Application.UnitTests
{
    public class GenerateResponseCommandHandlerTests
    {
        private readonly Mock<IAnswearGeneratorFactory> _mockFactory;
        private readonly Mock<IAnswearGenerator> _mockGenerator;
        private readonly GenerateResponseCommandHandler _handler;

        public GenerateResponseCommandHandlerTests()
        {
            _mockFactory = new Mock<IAnswearGeneratorFactory>();
            _mockGenerator = new Mock<IAnswearGenerator>();

            // By default, whenever the factory is asked to create a generator,
            // return our mocked generator.
            _mockFactory.Setup(f => f.CreateAnswearGenerator(It.IsAny<Dictionary<string, object>>()))
                        .Returns(_mockGenerator.Object);

            _handler = new GenerateResponseCommandHandler(_mockFactory.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenMessageCreationFails_DueToEmptyQuestion()
        {
            // Arrange: Provide an empty question to force Message.Create to fail.
            var command = new GenerateResponseCommand
            {
                Question = "",
                RetrievedData = "Some data",
                PromptWrapper = "Wrapper with {retrievedData} and {question}",
                Parameters = new Dictionary<string, object>()
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(MessageErrors.QuestionEmpty, result.Error);

            // Verify that no call to the generator was made.
            _mockGenerator.Verify(g => g.GenerateAnswearAsync(It.IsAny<Message>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenMessageCreationFails_DueToInvalidPromptWrapper()
        {
            // Arrange: Provide a prompt wrapper that lacks required placeholders.
            var command = new GenerateResponseCommand
            {
                Question = "What is your name?",
                RetrievedData = "Some data",
                PromptWrapper = "Invalid prompt wrapper", // Missing both "{retrievedData}" and "{question}"
                Parameters = new Dictionary<string, object>()
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            // Because the Message.Create method checks for {retrievedData} first:
            Assert.Equal(MessageErrors.InvalidPromptWrapper("{retrievedData}"), result.Error);

            // Generator should not be called when message creation fails.
            _mockGenerator.Verify(g => g.GenerateAnswearAsync(It.IsAny<Message>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenGeneratorReturnsFailure()
        {
            // Arrange: Create a valid command.
            var command = new GenerateResponseCommand
            {
                Question = "What is your name?",
                RetrievedData = "User data",
                PromptWrapper = "Wrapper: {retrievedData} | {question}",
                Parameters = new Dictionary<string, object>()
            };

            // Setup the generator to return a failure result.
            _mockGenerator.Setup(g => g.GenerateAnswearAsync(It.IsAny<Message>()))
                          .ReturnsAsync(Result<string>.Failure(MessageErrors.QuestionEmpty));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(MessageErrors.QuestionEmpty, result.Error);

            // Verify that the generator factory and generator were both invoked.
            _mockFactory.Verify(f => f.CreateAnswearGenerator(command.Parameters), Times.Once);
            _mockGenerator.Verify(g => g.GenerateAnswearAsync(It.IsAny<Message>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenGeneratorReturnsSuccess()
        {
            // Arrange: Create a valid command.
            var command = new GenerateResponseCommand
            {
                Question = "What is your name?",
                RetrievedData = "User data",
                PromptWrapper = "Wrapper: {retrievedData} | {question}",
                Parameters = new Dictionary<string, object>()
            };

            // Setup the generator to return a success result.
            _mockGenerator.Setup(g => g.GenerateAnswearAsync(It.IsAny<Message>()))
                          .ReturnsAsync(Result<string>.Success("Successful response"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Successful response", result.Data);

            // Verify that the generator factory and generator were both invoked.
            _mockFactory.Verify(f => f.CreateAnswearGenerator(command.Parameters), Times.Once);
            _mockGenerator.Verify(g => g.GenerateAnswearAsync(It.IsAny<Message>()), Times.Once);
        }
    }
}
