using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Finance.Command.Transaction.PatchTransaction;

public class PatchTransactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, ILogger<PatchTransactionCommandHandler> logger)
    : IRequestHandler<PatchTransactionCommand, Result>
{
    public async Task<Result> Handle(PatchTransactionCommand command, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetTransactionById(command.Id);
        if(transaction is null)
        {
            logger.LogWarning("Transaction with ID {TransactionId} not found", command.Id);
            return Result.Failure("Recurring Transaction not found.", ErrorType.NotFound);
        }
        
        var result = transaction.Patch(command.Type, command.Description);
        if (result.IsFailure)
        {
            logger.LogWarning("Failed to patch transaction {TransactionId}", command.Id);
            return result;
        }
            
        
        await transactionRepository.UpdateTransaction(transaction);
        await unitOfWork.CommitAsync();
        return Result.Success();
    }
}