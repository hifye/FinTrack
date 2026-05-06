using Application.Abstraction.Persistance.Repositories.Auth;
using Application.Features.Auth.Commands.User.Login;
using Application.Features.Auth.Responses;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Application.Settings;
using Domain.Common;
using Domain.Entities.Auth;
using Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Application.UnitTests.Features.Auth.Commands.User.Login;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<ILogger<LoginCommandHandler>> _loggerMock;
    private readonly IOptions<JwtSettings> _jwtSettings;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _tokenServiceMock = new Mock<ITokenService>();
        _loggerMock = new Mock<ILogger<LoginCommandHandler>>();
        
        var settings = new JwtSettings { RefreshTokenExpirationInDays = 7 };
        _jwtSettings = Options.Create(settings);

        _handler = new LoginCommandHandler(
            _userRepositoryMock.Object,
            _refreshTokenRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _passwordHasherMock.Object,
            _tokenServiceMock.Object,
            _loggerMock.Object,
            _jwtSettings);
    }

    [Fact]
    public async Task Handle_DeveRetornarSucesso_QuandoCredenciaisSaoValidas()
    {
        // Arrange
        var command = new LoginCommand("teste@exemplo.com", "Senha123!");
        var user = Domain.Entities.Auth.User.Create("Teste", "teste@exemplo.com", "hashed_password").Value;
        
        _userRepositoryMock.Setup(x => x.GetUserByEmail(It.IsAny<Email>())).ReturnsAsync(user);
        _passwordHasherMock.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _tokenServiceMock.Setup(x => x.GenerateToken(It.IsAny<Domain.Entities.Auth.User>())).Returns("access_token");
        _tokenServiceMock.Setup(x => x.GenerateRefreshToken()).Returns("refresh_token");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Token.Should().Be("access_token");
        _refreshTokenRepositoryMock.Verify(x => x.RevokeAllUserTokens(user!.Id, It.IsAny<CancellationToken>()), Times.Once);
        _refreshTokenRepositoryMock.Verify(x => x.CreateRefreshToken(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveRetornarFalha_QuandoSenhaIncorreta()
    {
        // Arrange
        var command = new LoginCommand("teste@exemplo.com", "SenhaErrada");
        var user = Domain.Entities.Auth.User.Create("Teste", "teste@exemplo.com", "hashed_password").Value;

        _userRepositoryMock.Setup(x => x.GetUserByEmail(It.IsAny<Email>())).ReturnsAsync(user);
        _passwordHasherMock.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Invalid credentials");
    }
}
