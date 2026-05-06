using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Finance.Command.RecurringTransaction.DeleteRecurringTransaction;

public class DeleteRecurringTransactionCommandHandler(
    IRecurringTransactionRepository recurringTransactionRepository,
    IUnitOfWork unitOfWork, ILogger<DeleteRecurringTransactionCommandHandler> logger)
    : IRequestHandler<DeleteRecurringTransactionCommand, Result>
{
    public async Task<Result> Handle(DeleteRecurringTransactionCommand command, CancellationToken cancellationToken)
    {
        var deleted = await recurringTransactionRepository.DeleteRecurringTransaction(command.Id);
        if (!deleted)
        {
            logger.LogWarning("Recurring Transaction with ID {RecurringTransactionId} not found", command.Id);
            return Result.Failure("Recurring Transaction not found.", ErrorType.NotFound);
        }
            
        await unitOfWork.CommitAsync();
        return Result.Success();
    }
}