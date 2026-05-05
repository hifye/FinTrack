using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.RecurringTransaction.DeleteRecurringTransaction;

public class DeleteRecurringTransactionCommandHandler(
    IRecurringTransactionRepository recurringTransactionRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteRecurringTransactionCommand, Result>
{
    public async Task<Result> Handle(DeleteRecurringTransactionCommand command, CancellationToken cancellationToken)
    {
        var deleted = await recurringTransactionRepository.DeleteRecurringTransaction(command.Id);
        if (!deleted)
            return Result.Failure("Recurring Transaction not found.");
            
        await unitOfWork.CommitAsync();
        return Result.Success();
    }
}