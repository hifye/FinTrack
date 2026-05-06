using Domain.Entities.Finance;
using FluentAssertions;

namespace Domain.UnitTests.Entities.Finance;

/// <summary>
/// Contém testes unitários para a entidade <see cref="Transaction"/>.
/// </summary>
public class TransactionTests
{
    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _accountId = Guid.NewGuid();
    private readonly Guid _categoryId = Guid.NewGuid();
    private readonly Guid _recurringId = Guid.NewGuid();
    private readonly decimal _amount = 50.00m;
    private readonly string _type = "Gasto";
    private readonly string _description = "Lanche";

    [Fact]
    public void Create_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Act
        var result = Transaction.Create(_userId, _accountId, _categoryId, _recurringId, _amount, _type, _description);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Amount.Value.Should().Be(_amount);
        result.Value.Description.Should().Be(_description);
    }

    [Fact]
    public void Patch_ShouldUpdateDescription_WhenProvided()
    {
        // Arrange
        var transaction = Transaction.Create(_userId, _accountId, _categoryId, _recurringId, _amount, _type, _description).Value!;
        var newDescription = "Almoço";

        // Act
        var result = transaction.Patch(null, newDescription);

        // Assert
        result.IsSuccess.Should().BeTrue();
        transaction.Description.Should().Be(newDescription);
    }
}
