using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.Transaction.DeleteTransaction;
    
public class DeleteTransactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteTransactionCommand, Result>
{
    public async Task<Result> Handle(DeleteTransactionCommand command, CancellationToken cancellationToken)
    {
        var deleted = await transactionRepository.DeleteTransaction(command.Id);
        if (!deleted)
            return Result.Failure("Transaction not found.");
            
        await unitOfWork.CommitAsync();
        return Result.Success();
    }
}