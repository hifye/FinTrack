using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.ValueObjects;

/// <summary>
/// Contém testes unitários para o objeto de valor <see cref="Price"/>.
/// </summary>
public class PriceTests
{
    /// <summary>
    /// Verifica se o método Create retorna sucesso quando um valor positivo é fornecido.
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(100.50)]
    [InlineData(999999.99)]
    public void Create_ShouldReturnSuccess_WhenValueIsPositive(decimal validValue)
    {
        // Act
        var result = Price.Create(validValue);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Value.Should().Be(validValue);
    }

    /// <summary>
    /// Verifica se o método Create retorna falha quando o valor é menor que 1.
    /// </summary>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100.50)]
    public void Create_ShouldReturnFailure_WhenValueIsLessThanOne(decimal invalidValue)
    {
        // Act
        var result = Price.Create(invalidValue);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Price cannot be negative.");
    }
}
