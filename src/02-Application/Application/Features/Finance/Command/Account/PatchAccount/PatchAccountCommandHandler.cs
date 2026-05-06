using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Finance.Command.Account.PatchAccount;

public class PatchAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, ILogger<PatchAccountCommandHandler> logger)
    : IRequestHandler<PatchAccountCommand, Result>
{
    public async Task<Result> Handle(PatchAccountCommand command, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetAccountById(command.Id);
        if (account is null)
        {
            logger.LogWarning("Account with ID {AccountId} not found", command.Id);
            return Result.Failure("Account not found.", ErrorType.NotFound);
        }

        var result = account.Patch(command.Type, command.IsActive);
        if (result.IsFailure)
        {
            logger.LogWarning("Failed to patch account {AccountId}", command.Id);
            return result;
        }

        await accountRepository.UpdateAccount(account);
        await unitOfWork.CommitAsync();
        return Result.Success();
    }
}