using Domain.Common;
using FluentAssertions;

namespace Domain.UnitTests.Common;

/// <summary>
/// Contém testes unitários para a classe <see cref="Result"/>.
/// </summary>
public class ResultTests
{
    [Fact]
    public void Success_ShouldReturnSuccessResult()
    {
        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().BeNull();
        result.ErrorType.Should().Be(ErrorType.None);
    }

    [Fact]
    public void Failure_ShouldReturnFailureResult()
    {
        // Arrange
        var errorMessage = "Error occurred";
        var errorType = ErrorType.Validation;

        // Act
        var result = Result.Failure(errorMessage, errorType);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(errorMessage);
        result.ErrorType.Should().Be(errorType);
    }

    [Fact]
    public void Bind_ShouldExecuteFunc_WhenSuccess()
    {
        // Arrange
        var result = Result.Success();
        var executed = false;

        // Act
        var nextResult = result.Bind(() => {
            executed = true;
            return Result.Success();
        });

        // Assert
        executed.Should().BeTrue();
        nextResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Bind_ShouldNotExecuteFunc_WhenFailure()
    {
        // Arrange
        var result = Result.Failure("Error", ErrorType.Validation);
        var executed = false;

        // Act
        var nextResult = result.Bind(() => {
            executed = true;
            return Result.Success();
        });

        // Assert
        executed.Should().BeFalse();
        nextResult.IsFailure.Should().BeTrue();
        nextResult.Error.Should().Be("Error");
    }

    [Fact]
    public void Map_ShouldTransformValue_WhenSuccess()
    {
        // Arrange
        var result = Result<int>.Success(10);

        // Act
        var nextResult = result.Map(v => v.ToString());

        // Assert
        nextResult.IsSuccess.Should().BeTrue();
        nextResult.Value.Should().Be("10");
    }

    [Fact]
    public void Try_ShouldReturnSuccess_WhenNoException()
    {
        // Act
        var result = Result.Try(() => { }, "Error");

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Try_ShouldReturnFailure_WhenExceptionOccurs()
    {
        // Act
        var result = Result.Try(() => throw new Exception(), "Error message");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Error message");
        result.ErrorType.Should().Be(ErrorType.Unexpected);
    }
}
