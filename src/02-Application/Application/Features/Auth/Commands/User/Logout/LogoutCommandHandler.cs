using Application.Abstraction.Persistance.Repositories.Auth;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;

namespace Application.Features.Auth.Commands.User.Logout;

public class LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
    : IRequestHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        await refreshTokenRepository.RevokeAllUserTokens(currentUser.UserId, cancellationToken);
        await unitOfWork.CommitAsync();
        return Result.Success();
    }
}