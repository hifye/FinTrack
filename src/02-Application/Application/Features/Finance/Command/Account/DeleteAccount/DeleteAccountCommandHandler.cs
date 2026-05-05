using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.Account.DeleteAccount;

public class DeleteAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteAccountCommand, Result>
{
    public async Task<Result> Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
    {
        var deleted = await accountRepository.DeleteAccount(command.Id);
        if (!deleted)
            return Result.Failure("Category not found.");
        
        await unitOfWork.CommitAsync();
        return Result.Success();
    }
}