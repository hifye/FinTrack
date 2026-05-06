using Application.Abstraction.Persistance.Repositories.Catalog;
using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Features.Finance.Command.Transaction.CreateTransaction;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using Domain.Entities.Catalog;
using Domain.Entities.Finance;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Features.Finance.Command.Transaction.CreateTransaction;

public class CreateTransactionCommandHandlerTests
{
    private readonly Mock<IRecurringTransactionRepository> _recurringTransactionRepositoryMock;
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<CreateTransactionCommandHandler>> _loggerMock;
    private readonly CreateTransactionCommandHandler _handler;

    public CreateTransactionCommandHandlerTests()
    {
        _recurringTransactionRepositoryMock = new Mock<IRecurringTransactionRepository>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<CreateTransactionCommandHandler>>();

        _handler = new CreateTransactionCommandHandler(
            _recurringTransactionRepositoryMock.Object,
            _transactionRepositoryMock.Object,
            _categoryRepositoryMock.Object,
            _accountRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarSucesso_QuandoDadosSaoValidos()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var recurringId = Guid.NewGuid();
        
        var command = new CreateTransactionCommand(accountId, categoryId, recurringId, 100m, "Saída", "Almoço");

        var category = Category.Create(userId, "Alimentação", "Despesa").Value;
        var account = Domain.Entities.Finance.Account.Create(userId, "Carteira", "Dinheiro", 500m).Value;
        var recurring = RecurringTransaction.Create(userId, accountId, categoryId, 100m, "Saída", "Aluguel", "Mensal").Value;

        _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        _categoryRepositoryMock.Setup(x => x.GetCategoryById(categoryId)).ReturnsAsync(category);
        _accountRepositoryMock.Setup(x => x.GetAccountById(accountId)).ReturnsAsync(account);
        _recurringTransactionRepositoryMock.Setup(x => x.GetRecurringTransactionById(recurringId)).ReturnsAsync(recurring);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _transactionRepositoryMock.Verify(x => x.CreateTransaction(It.IsAny<Domain.Entities.Finance.Transaction>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveRetornarFalha_QuandoCategoriaNaoExiste()
    {
        // Arrange
        var command = new CreateTransactionCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 100m, "Saída", "Almoço");
        _categoryRepositoryMock.Setup(x => x.GetCategoryById(It.IsAny<Guid>())).ReturnsAsync((Category)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Category not found.");
        result.ErrorType.Should().Be(ErrorType.NotFound);
    }
}
