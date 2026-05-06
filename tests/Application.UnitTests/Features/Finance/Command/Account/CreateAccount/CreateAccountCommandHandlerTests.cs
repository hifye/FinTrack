using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Features.Finance.Command.Account.CreateAccount;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using Domain.Entities.Finance;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Features.Finance.Command.Account.CreateAccount;

public class CreateAccountCommandHandlerTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly CreateAccountCommandHandler _handler;

    public CreateAccountCommandHandlerTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _handler = new CreateAccountCommandHandler(
            _accountRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarSucesso_QuandoDadosSaoValidos()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateAccountCommand("Conta Corrente", "Corrente", 1000m);
        _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _accountRepositoryMock.Verify(x => x.CreateAccount(It.IsAny<Domain.Entities.Finance.Account>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveRetornarFalha_QuandoNomeEhMuitoLongo()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var longName = new string('a', 51);
        var command = new CreateAccountCommand(longName, "Corrente", 1000m);
        _currentUserServiceMock.Setup(x => x.UserId).Returns(userId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("The name cannot be longer than 50 characters.");
    }
}
