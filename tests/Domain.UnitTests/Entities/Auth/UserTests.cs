using Domain.Entities.Auth;
using FluentAssertions;

namespace Domain.UnitTests.Entities.Auth;

/// <summary>
/// Contém testes unitários para a entidade <see cref="User"/>.
/// </summary>
public class UserTests
{
    private readonly string _name = "João Silva";
    private readonly string _email = "joao@example.com";
    private readonly string _passwordHash = "hashed_password";

    [Fact]
    public void Create_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Act
        var result = User.Create(_name, _email, _passwordHash);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Name.Should().Be(_name);
        result.Value.Email.Address.Should().Be(_email);
        result.Value.PasswordHash.Should().Be(_passwordHash);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenEmailIsInvalid()
    {
        // Act
        var result = User.Create(_name, "invalid-email", _passwordHash);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Invalid Email");
    }

    [Fact]
    public void UpdatePassword_ShouldChangePasswordAndSetUpdatedAt()
    {
        // Arrange
        var user = User.Create(_name, _email, _passwordHash).Value!;
        var newPassword = "new_hashed_password";

        // Act
        var result = user.UpdatePassword(newPassword);

        // Assert
        result.IsSuccess.Should().BeTrue();
        user.PasswordHash.Should().Be(newPassword);
        user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}
