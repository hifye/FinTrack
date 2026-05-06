using Domain.Entities.Finance;
using FluentAssertions;

namespace Domain.UnitTests.Entities.Finance;

/// <summary>
/// Contém testes unitários para a entidade <see cref="Account"/>.
/// </summary>
public class AccountTests
{
    private readonly Guid _userId = Guid.NewGuid();
    private readonly string _name = "Minha Conta";
    private readonly string _type = "Corrente";
    private readonly decimal _initialBalance = 1000m;

    [Fact]
    public void Create_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Act
        var result = Account.Create(_userId, _name, _type, _initialBalance);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.UserId.Should().Be(_userId);
        result.Value.Name.Should().Be(_name);
        result.Value.Type.Should().Be(_type);
        result.Value.InitialBalance.Value.Should().Be(_initialBalance);
        result.Value.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenUserIdIsEmpty()
    {
        // Act
        var result = Account.Create(Guid.Empty, _name, _type, _initialBalance);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("The field User id is mandatory.");
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenNameIsTooLong()
    {
        // Arrange
        var longName = new string('a', 51);

        // Act
        var result = Account.Create(_userId, longName, _type, _initialBalance);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("The name cannot be longer than 50 characters.");
    }

    [Fact]
    public void Patch_ShouldUpdateFields_WhenDataIsValid()
    {
        // Arrange
        var account = Account.Create(_userId, _name, _type, _initialBalance).Value!;
        var newType = "Poupanca";

        // Act
        var result = account.Patch(newType, false);

        // Assert
        result.IsSuccess.Should().BeTrue();
        account.Type.Should().Be(newType);
        account.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Patch_ShouldReturnFailure_WhenNoFieldsProvided()
    {
        // Arrange
        var account = Account.Create(_userId, _name, _type, _initialBalance).Value!;

        // Act
        var result = account.Patch(null, null);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("At least one field must be provided for patching.");
    }
}
