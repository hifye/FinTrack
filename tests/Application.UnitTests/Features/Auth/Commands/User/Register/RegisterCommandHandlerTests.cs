using Application.Abstraction.Persistance.Repositories.Auth;
using Application.Features.Auth.Commands.User.Register;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using Domain.Entities.Auth;
using Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Features.Auth.Commands.User.Register;

/// <summary>
/// Testes unitários para o tratador do comando de registro de usuário.
/// </summary>
public class RegisterCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<ILogger<RegisterCommandHandler>> _loggerMock;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _loggerMock = new Mock<ILogger<RegisterCommandHandler>>();
        _handler = new RegisterCommandHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _passwordHasherMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarSucesso_QuandoDadosSaoValidos()
    {
        // Arrange
        var command = new RegisterCommand("Teste", "teste@exemplo.com", "Senha123!", "Senha123!");
        _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashed_password");
        _userRepositoryMock.Setup(x => x.GetUserByEmail(It.IsAny<Email>())).ReturnsAsync((Domain.Entities.Auth.User)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _userRepositoryMock.Verify(x => x.CreateUser(It.IsAny<Domain.Entities.Auth.User>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveRetornarFalha_QuandoEmailJaExiste()
    {
        // Arrange
        var command = new RegisterCommand("Teste", "teste@exemplo.com", "Senha123!", "Senha123!");
        _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashed_password");
        
        var existingUser = Domain.Entities.Auth.User.Create("Existente", "teste@exemplo.com", "hash").Value;
        _userRepositoryMock.Setup(x => x.GetUserByEmail(It.IsAny<Email>())).ReturnsAsync(existingUser);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Email already exists");
        result.ErrorType.Should().Be(ErrorType.Conflict);
        _userRepositoryMock.Verify(x => x.CreateUser(It.IsAny<Domain.Entities.Auth.User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_DeveRetornarFalha_QuandoEmailForInvalido()
    {
        // Arrange
        var command = new RegisterCommand("Teste", "email-invalido", "Senha123!", "Senha123!");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ErrorType.Validation);
    }
}
