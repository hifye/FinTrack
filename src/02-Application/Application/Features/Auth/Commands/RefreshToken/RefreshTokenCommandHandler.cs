using Application.Abstraction.Persistance.Repositories.Auth;
using Application.Features.Auth.Responses;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Application.Settings;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ITokenService tokenService,
    IOptions<JwtSettings> jwtSettings)
    : IRequestHandler<RefreshTokenCommand, Result<TokenResponse>>
{
    public async Task<Result<TokenResponse>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var existing  = await refreshTokenRepository.GetRefreshToken(command.RefreshToken, cancellationToken);
        if(existing is null)
            return Result<TokenResponse>.Failure("Invalid or expired refresh token.", ErrorType.Validation);
        
        var user = await userRepository.GetUserById(existing.UserId);
        if(user is null)
            return Result<TokenResponse>.Failure("User not found.", ErrorType.NotFound);

        var revokeResult = existing.Revoke();
        if(revokeResult.IsFailure)
            return Result<TokenResponse>.Failure(revokeResult.Error!, ErrorType.Validation);

        await refreshTokenRepository.RevokeRefreshToken(existing.Id, cancellationToken);
        
        var accessToken = tokenService.GenerateToken(user);
        var rawRefreshToken = tokenService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddDays(jwtSettings.Value.RefreshTokenExpirationInDays);
        
        var newRefreshToken = Domain.Entities.Auth.RefreshToken.Create(user.Id, rawRefreshToken, expiresAt);
        if (newRefreshToken.IsFailure)
            return Result<TokenResponse>.Failure(newRefreshToken.Error!, ErrorType.Validation);
        
        await refreshTokenRepository.CreateRefreshToken(newRefreshToken.Value!, cancellationToken);
        await unitOfWork.CommitAsync();
        
        return Result<TokenResponse>.Success(new TokenResponse(accessToken, rawRefreshToken, expiresAt));
    }
}