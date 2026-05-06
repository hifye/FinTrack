using Application.Abstraction.Persistance.Repositories.Catalog;
using Application.Features.Catalog.Commands.Category.DeleteCategory;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Features.Catalog.Commands.Category.DeleteCategory;

/// <summary>
/// Contém testes unitários para o <see cref="DeleteCategoryCommandHandler"/>.
/// </summary>
public class DeleteCategoryCommandHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<DeleteCategoryCommandHandler>> _loggerMock;
    private readonly DeleteCategoryCommandHandler _handler;

    public DeleteCategoryCommandHandlerTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<DeleteCategoryCommandHandler>>();
        _handler = new DeleteCategoryCommandHandler(
            _categoryRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCategoryExists()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new DeleteCategoryCommand(categoryId);
        _categoryRepositoryMock.Setup(r => r.DeleteCategory(categoryId))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _categoryRepositoryMock.Verify(r => r.DeleteCategory(categoryId), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new DeleteCategoryCommand(categoryId);
        _categoryRepositoryMock.Setup(r => r.DeleteCategory(categoryId))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Category not found.");
        result.ErrorType.Should().Be(ErrorType.NotFound);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
    }
}
