using Application.Abstraction.Persistance.Repositories.Catalog;
using Application.Features.Catalog.Commands.Category.CreateCategory;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Features.Catalog.Commands.Category.CreateCategory;

/// <summary>
/// Contém testes unitários para o <see cref="CreateCategoryCommandHandler"/>.
/// </summary>
public class CreateCategoryCommandHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly CreateCategoryCommandHandler _handler;

    public CreateCategoryCommandHandlerTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _handler = new CreateCategoryCommandHandler(
            _categoryRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateCategoryCommand("Alimentação", "Despesa");
        _currentUserServiceMock.Setup(s => s.UserId).Returns(userId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _categoryRepositoryMock.Verify(r => r.CreateCategory(It.IsAny<Domain.Entities.Catalog.Category>()), Times.Once());
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var command = new CreateCategoryCommand("", "Despesa"); // Nome vazio falha na validação do domínio

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        _categoryRepositoryMock.Verify(r => r.CreateCategory(It.IsAny<Domain.Entities.Catalog.Category>()), Times.Never());
    }
}
