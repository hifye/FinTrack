using Application.Abstraction.Persistance.Repositories.Auth;
using Application.Features.Auth.Responses;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Application.Settings;
using Domain.Common;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Features.Auth.Commands.User.Login;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    ILogger<LoginCommandHandler> logger,
    IOptions<JwtSettings> jwtSettings) : IRequestHandler<LoginCommand, Result<TokenResponse>>
{
    public async Task<Result<TokenResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(command.Email);
        if (emailResult.IsFailure)
            return Result<TokenResponse>.Failure(emailResult.Error!, ErrorType.Validation);

        var user = await userRepository.GetUserByEmail(emailResult.Value!);
        if (user is null || !passwordHasher.VerifyPassword(command.Password, user.PasswordHash))
        {
            logger.LogWarning("Invalid credentials for user {Email}", emailResult.Value);
            return Result<TokenResponse>.Failure("Invalid credentials", ErrorType.Validation);
        }


        if (passwordHasher.NeedsRehash(user.PasswordHash))
        {
            var hash = passwordHasher.HashPassword(command.Password);
            user.PasswordHash = hash;
            await userRepository.UpdateUser(user);
        }

        await refreshTokenRepository.RevokeAllUserTokens(user.Id, cancellationToken);

        var accessToken = tokenService.GenerateToken(user);
        var refreshToken = tokenService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddDays(jwtSettings.Value.RefreshTokenExpirationInDays);

        var refreshTokenResult = Domain.Entities.Auth.RefreshToken.Create(user.Id, refreshToken, expiresAt);
        if (refreshTokenResult.IsFailure)
        {
            logger.LogWarning("Failed to create refresh token for user {UserId}", user.Id);
            return Result<TokenResponse>.Failure(refreshTokenResult.Error!, ErrorType.Validation);
        }
        
        await refreshTokenRepository.CreateRefreshToken(refreshTokenResult.Value!, cancellationToken);
        await unitOfWork.CommitAsync();

        return Result<TokenResponse>.Success(new TokenResponse(accessToken, refreshToken, expiresAt));
    }
}