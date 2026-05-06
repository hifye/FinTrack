using Domain.Common;
using FluentAssertions;

namespace Domain.UnitTests.Common;

/// <summary>
/// Contém testes unitários para a classe utilitária <see cref="Guard"/>.
/// </summary>
public class GuardTests
{
    /// <summary>
    /// Verifica se AgainstNullOrWhiteSpace retorna sucesso para uma string válida.
    /// </summary>
    [Fact]
    public void AgainstNullOrWhiteSpace_ShouldReturnSuccess_WhenValueIsPresent()
    {
        // Act
        var result = Guard.AgainstNullOrWhiteSpace("valid", "Error message");

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    /// <summary>
    /// Verifica se AgainstNullOrWhiteSpace retorna falha para nulo ou espaço em branco.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AgainstNullOrWhiteSpace_ShouldReturnFailure_WhenValueIsMissing(string? invalidValue)
    {
        // Arrange
        var errorMessage = "Value is required";

        // Act
        var result = Guard.AgainstNullOrWhiteSpace(invalidValue!, errorMessage);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(errorMessage);
        result.ErrorType.Should().Be(ErrorType.Validation);
    }

    /// <summary>
    /// Verifica se AgainstOutOfRange retorna sucesso quando a condição é falsa.
    /// </summary>
    [Fact]
    public void AgainstOutOfRange_ShouldReturnSuccess_WhenConditionIsFalse()
    {
        // Act
        var result = Guard.AgainstOutOfRange(false, "Error message");

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    /// <summary>
    /// Verifica se AgainstOutOfRange retorna falha quando a condição é verdadeira.
    /// </summary>
    [Fact]
    public void AgainstOutOfRange_ShouldReturnFailure_WhenConditionIsTrue()
    {
        // Arrange
        var errorMessage = "Out of range";

        // Act
        var result = Guard.AgainstOutOfRange(true, errorMessage);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(errorMessage);
        result.ErrorType.Should().Be(ErrorType.Validation);
    }
}
