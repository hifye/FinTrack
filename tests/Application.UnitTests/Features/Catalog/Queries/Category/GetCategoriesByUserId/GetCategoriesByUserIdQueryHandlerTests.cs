using Application.Abstraction.Queries;
using Application.Features.Catalog.ListItem;
using Application.Features.Catalog.Queries.Category.GetCategoriesByUserId;
using Application.Interfaces.Services;
using Domain.Common;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Features.Catalog.Queries.Category.GetCategoriesByUserId;

public class GetCategoriesByUserIdQueryHandlerTests
{
    private readonly Mock<ICategoryQueries> _categoryQueriesMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<ILogger<GetCategoriesByUserIdQueryHandler>> _loggerMock;
    private readonly GetCategoriesByUserIdQueryHandler _handler;

    public GetCategoriesByUserIdQueryHandlerTests()
    {
        _categoryQueriesMock = new Mock<ICategoryQueries>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _loggerMock = new Mock<ILogger<GetCategoriesByUserIdQueryHandler>>();
        _handler = new GetCategoriesByUserIdQueryHandler(
            _categoryQueriesMock.Object,
            _currentUserServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarSucesso_QuandoCategoriasExistem()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var categories = new List<CategoryListItem>
        {
            new CategoryListItem(Guid.NewGuid(), "Alimentação", "Despesa", true, DateTime.UtcNow),
            new CategoryListItem(Guid.NewGuid(), "Salário", "Receita", true, DateTime.UtcNow)
        };

        _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        _categoryQueriesMock.Setup(x => x.GetCategoriesByUserId(userId)).ReturnsAsync(categories);

        // Act
        var result = await _handler.Handle(new GetCategoriesByUserIdQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_DeveRetornarFalha_QuandoNenhumaCategoriaEhEncontrada()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        _categoryQueriesMock.Setup(x => x.GetCategoriesByUserId(userId)).ReturnsAsync(new List<CategoryListItem>());

        // Act
        var result = await _handler.Handle(new GetCategoriesByUserIdQuery(), CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("No categories found for the user");
        result.ErrorType.Should().Be(ErrorType.NotFound);
    }
}
