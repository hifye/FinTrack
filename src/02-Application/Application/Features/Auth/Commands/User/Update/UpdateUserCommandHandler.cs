using Application.Abstraction.Persistance.Repositories.Auth;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;

namespace Application.Features.Auth.Commands.User.Update;

public class UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser, IPasswordHasher passwordHasher)
    : IRequestHandler<UpdateUserCommand, Result>
{
    public async Task<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserById(currentUser.UserId);
        if(user is null)
            return Result.Failure("User not found");
        
        var hash = passwordHasher.HashPassword(command.Password);
        
        var result = user.Update(hash);
        if(result.IsFailure)
            return Result.Failure("Failed to update user");
        
        await userRepository.UpdateUser(user);
        await unitOfWork.CommitAsync();
        return Result.Success();
    }
}