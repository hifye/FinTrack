using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.ValueObjects;

/// <summary>
/// Contém testes unitários para o objeto de valor <see cref="Email"/>.
/// </summary>
public class EmailTests
{
    /// <summary>
    /// Verifica se o método Create retorna sucesso quando um e-mail válido é fornecido.
    /// </summary>
    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name@domain.co.uk")]
    [InlineData("someone123@gmail.com")]
    public void Create_ShouldReturnSuccess_WhenEmailIsValid(string validEmail)
    {
        // Act
        var result = Email.Create(validEmail);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Address.Should().Be(validEmail.ToLower());
    }

    /// <summary>
    /// Verifica se o método Create retorna falha quando o e-mail é nulo ou vazio.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_ShouldReturnFailure_WhenEmailIsEmpty(string? emptyEmail)
    {
        // Act
        var result = Email.Create(emptyEmail!);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Email cannot be empty");
    }

    /// <summary>
    /// Verifica se o método Create retorna falha quando o e-mail excede 100 caracteres.
    /// </summary>
    [Fact]
    public void Create_ShouldReturnFailure_WhenEmailIsTooLong()
    {
        // Arrange
        var longEmail = new string('a', 92) + "@test.com"; // 92 + 9 = 101 caracteres

        // Act
        var result = Email.Create(longEmail);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Email cannot be longer than 100 characters.");
    }

    /// <summary>
    /// Verifica se o método Create retorna falha quando o formato do e-mail é inválido.
    /// </summary>
    [Theory]
    [InlineData("invalid-email")]
    [InlineData("test@")]
    [InlineData("@example.com")]
    public void Create_ShouldReturnFailure_WhenEmailFormatIsInvalid(string invalidEmail)
    {
        // Act
        var result = Email.Create(invalidEmail);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Invalid Email");
    }

    /// <summary>
    /// Verifica se o e-mail é normalizado para letras minúsculas e espaços removidos.
    /// </summary>
    [Fact]
    public void Create_ShouldNormalizeEmail_WhenEmailHasUpperCaseOrSpaces()
    {
        // Arrange
        var inputEmail = "  TEST@Example.Com  ";
        var expectedEmail = "test@example.com";

        // Act
        var result = Email.Create(inputEmail);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Address.Should().Be(expectedEmail);
    }
}
