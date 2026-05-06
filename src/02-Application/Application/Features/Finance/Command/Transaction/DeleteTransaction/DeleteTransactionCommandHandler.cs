using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Finance.Command.Transaction.DeleteTransaction;
    
public class DeleteTransactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, ILogger<DeleteTransactionCommandHandler> logger)
    : IRequestHandler<DeleteTransactionCommand, Result>
{
    public async Task<Result> Handle(DeleteTransactionCommand command, CancellationToken cancellationToken)
    {
        var deleted = await transactionRepository.DeleteTransaction(command.Id);
        if (!deleted)
        {
            logger.LogWarning("Transaction with ID {TransactionId} not found", command.Id);
            return Result.Failure("Transaction not found.", ErrorType.NotFound);
        }
            
        await unitOfWork.CommitAsync();
        return Result.Success();
    }
}