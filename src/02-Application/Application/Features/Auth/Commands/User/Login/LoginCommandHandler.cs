using Application.Abstraction.Persistance.Repositories.Auth;
using Application.Features.Auth.Commands.User.Login;
using Application.Features.Auth.Responses;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using Domain.Entities.Auth;
using Domain.ValueObjects;
using Infrastructure.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Features.Auth.Commands.User.Login;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    IOptions<JwtSettings> jwtSettings) : IRequestHandler<LoginCommand, Result<TokenResponse>>
{
    public async Task<Result<TokenResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(command.Email);
        if(emailResult.IsFailure)
            return Result<TokenResponse>.Failure(emailResult.Error!);

        var user = await userRepository.GetUserByEmail(emailResult.Value!);
        if(user is null || !passwordHasher.VerifyPassword(command.Password, user.PasswordHash))
            return Result<TokenResponse>.Failure("Invalid credentials");
        
        await refreshTokenRepository.RevokeAllUserTokens(user.Id, cancellationToken);
        
        var accessToken = tokenService.GenerateToken(user);
        var refreshToken = tokenService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddDays(jwtSettings.Value.RefreshTokenExpirationInDays);
        
        var refreshTokenResult = Domain.Entities.Auth.RefreshToken.Create(user.Id, refreshToken, expiresAt);
        if(refreshTokenResult.IsFailure)
            return Result<TokenResponse>.Failure(refreshTokenResult.Error!);

        await refreshTokenRepository.CreateRefreshToken(refreshTokenResult.Value!, cancellationToken);
        await unitOfWork.CommitAsync();
        
        return Result<TokenResponse>.Success(new TokenResponse(accessToken, refreshToken, expiresAt));
    }
}