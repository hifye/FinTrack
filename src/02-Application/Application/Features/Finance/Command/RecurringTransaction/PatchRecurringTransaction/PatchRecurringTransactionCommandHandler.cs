using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.RecurringTransaction.PatchRecurringTransaction;

public class PatchRecurringTransactionCommandHandler(
    IRecurringTransactionRepository recurringTransactionRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<PatchRecurringTransactionCommand, Result>
{
    public async Task<Result> Handle(PatchRecurringTransactionCommand command, CancellationToken cancellationToken)
    {
        var recurringTransaction = await recurringTransactionRepository.GetRecurringTransactionById(command.Id);
        if(recurringTransaction is null)
            return Result.Failure("Recurring Transaction not found.");
        
        var result = recurringTransaction.Patch(command.Amount, command.Type, command.Description, command.Frequency, command.IsActive);
        if (result.IsFailure)
            return result;
        
        await recurringTransactionRepository.UpdateRecurringTransaction(recurringTransaction);
        await unitOfWork.CommitAsync();
        return Result.Success();
    }
}