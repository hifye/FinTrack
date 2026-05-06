using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Finance.Command.Account.DeleteAccount;

public class DeleteAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, ILogger<DeleteAccountCommandHandler> logger)
    : IRequestHandler<DeleteAccountCommand, Result>
{
    public async Task<Result> Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
    {
        var deleted = await accountRepository.DeleteAccount(command.Id);
        if (!deleted)
        {
            logger.LogWarning("Account with ID {AccountId} not found", command.Id);
            return Result.Failure("Category not found.", ErrorType.NotFound);
        }
        
        await unitOfWork.CommitAsync();
        return Result.Success();
    }
}