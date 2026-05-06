using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Features.Finance.Command.Account.PatchAccount;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using Domain.Entities.Finance;
using Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Features.Finance.Command.Account.PatchAccount;

public class PatchAccountCommandHandlerTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<PatchAccountCommandHandler>> _loggerMock;
    private readonly PatchAccountCommandHandler _handler;

    public PatchAccountCommandHandlerTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<PatchAccountCommandHandler>>();
        _handler = new PatchAccountCommandHandler(
            _accountRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarSucesso_QuandoDadosSaoValidos()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var command = new PatchAccountCommand(accountId, "Novo Tipo", true);
        var account = Domain.Entities.Finance.Account.Create(Guid.NewGuid(), "Conta", "Corrente", 100m).Value;
        
        _accountRepositoryMock.Setup(x => x.GetAccountById(accountId)).ReturnsAsync(account);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        account!.Type.Should().Be("Novo Tipo");
        _accountRepositoryMock.Verify(x => x.UpdateAccount(It.IsAny<Domain.Entities.Finance.Account>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveRetornarFalha_QuandoContaNaoExiste()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var command = new PatchAccountCommand(accountId, "Novo Tipo", true);
        _accountRepositoryMock.Setup(x => x.GetAccountById(accountId)).ReturnsAsync((Domain.Entities.Finance.Account?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ErrorType.NotFound);
    }
}
