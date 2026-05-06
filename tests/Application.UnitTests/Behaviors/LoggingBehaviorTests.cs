using Application.Behaviors;
using Domain.Common;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Behaviors;

/// <summary>
/// Contém testes unitários para o <see cref="LoggingBehavior{TRequest,TResponse}"/>.
/// </summary>
public class LoggingBehaviorTests
{
    private readonly Mock<ILogger<LoggingBehavior<TestRequest, Result>>> _loggerMock;
    private readonly LoggingBehavior<TestRequest, Result> _behavior;

    public LoggingBehaviorTests()
    {
        _loggerMock = new Mock<ILogger<LoggingBehavior<TestRequest, Result>>>();
        _behavior = new LoggingBehavior<TestRequest, Result>(_loggerMock.Object);
    }

    public record TestRequest : IRequest<Result>;

    public interface INextDelegate
    {
        Task<Result> Invoke();
    }

    [Fact]
    public async Task Handle_ShouldLogInformation_WhenRequestIsSuccessful()
    {
        // Arrange
        var request = new TestRequest();
        Task<Result> Next() => Task.FromResult(Result.Success());
        
        // Act
        var result = await _behavior.Handle(request, _ => Next(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Handling TestRequest")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("completed successfully")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldLogWarning_WhenRequestFails()
    {
        // Arrange
        var request = new TestRequest();
        Task<Result> Next() => Task.FromResult(Result.Failure("Error", ErrorType.Validation));

        // Act
        var result = await _behavior.Handle(request, _ => Next(), CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("failed")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldLogErrorAndRethrow_WhenExceptionOccurs()
    {
        // Arrange
        var request = new TestRequest();
        var exception = new Exception("Test exception");
        Task<Result> Next() => throw exception;

        // Act
        Func<Task> act = async () => await _behavior.Handle(request, _ => Next(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Test exception");
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("threw an unhandled exception")),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
