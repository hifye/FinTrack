using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.Account.PatchAccount;

public class PatchAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<PatchAccountCommand, Result>
{
    public async Task<Result> Handle(PatchAccountCommand command, CancellationToken cancellationToken)
    {
        var category = await accountRepository.GetAccountById(command.Id);
        if(category is null)
            return Result.Failure("Category not found.");
        
        var result = category.Patch(command.Type, command.IsActive);
        if (result.IsFailure)
            return result;
        
        await accountRepository.UpdateAccount(category);
        await unitOfWork.CommitAsync();
        return Result.Success();
    }
}