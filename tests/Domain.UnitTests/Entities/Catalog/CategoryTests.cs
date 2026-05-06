using Domain.Entities.Catalog;
using FluentAssertions;

namespace Domain.UnitTests.Entities.Catalog;

/// <summary>
/// Contém testes unitários para a entidade <see cref="Category"/>.
/// </summary>
public class CategoryTests
{
    private readonly Guid _userId = Guid.NewGuid();
    private readonly string _name = "Alimentação";
    private readonly string _type = "Despesa";

    [Fact]
    public void Create_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Act
        var result = Category.Create(_userId, _name, _type);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.UserId.Should().Be(_userId);
        result.Value.Name.Should().Be(_name);
        result.Value.Type.Should().Be(_type);
        result.Value.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenNameIsEmpty()
    {
        // Act
        var result = Category.Create(_userId, "", _type);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("The field name is mandatory.");
    }

    [Fact]
    public void Patch_ShouldUpdateName_WhenNameIsProvided()
    {
        // Arrange
        var category = Category.Create(_userId, _name, _type).Value!;
        var newName = "Supermercado";

        // Act
        var result = category.Patch(newName, null, null);

        // Assert
        result.IsSuccess.Should().BeTrue();
        category.Name.Should().Be(newName);
    }

    [Fact]
    public void Patch_ShouldUpdateIsActive_WhenIsActiveIsProvided()
    {
        // Arrange
        var category = Category.Create(_userId, _name, _type).Value!;

        // Act
        var result = category.Patch(null, null, false);

        // Assert
        result.IsSuccess.Should().BeTrue();
        category.IsActive.Should().BeFalse();
    }
}
