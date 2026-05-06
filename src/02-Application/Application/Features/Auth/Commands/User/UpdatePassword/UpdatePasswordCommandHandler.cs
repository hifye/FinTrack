using Application.Abstraction.Persistance.Repositories.Auth;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Auth.Commands.User.UpdatePassword;

public class UpdatePasswordCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser,
    IPasswordHasher passwordHasher,
    ILogger<UpdatePasswordCommandHandler> logger
) : IRequestHandler<UpdatePasswordCommand, Result>
{
    public async Task<Result> Handle(
        UpdatePasswordCommand command,
        CancellationToken cancellationToken
    )
    {
        var user = await userRepository.GetUserById(currentUser.UserId);
        if (user is null)
        {
            logger.LogWarning("User not found for id {UserId}", currentUser.UserId);
            return Result.Failure("User not found", ErrorType.NotFound);
        }

        var hash = passwordHasher.HashPassword(command.Password);

        var result = user.UpdatePassword(hash);
        if (result.IsFailure)
        {
            logger.LogWarning("Failed to update password for user {UserId}", currentUser.UserId);
            return Result.Failure("Failed to update password", ErrorType.Validation);
        }

        await userRepository.UpdateUser(user);
        await unitOfWork.CommitAsync();
        return Result.Success();
    }
}
