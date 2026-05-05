using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.Transaction.PatchTransaction;

public class PatchTransactionCommandHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<PatchTransactionCommand, Result>
{
    public async Task<Result> Handle(PatchTransactionCommand command, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetTransactionById(command.Id);
        if(transaction is null)
            return Result.Failure("Recurring Transaction not found.");
        
        var result = transaction.Patch(command.Type, command.Description);
        if (result.IsFailure)
            return result;
        
        await transactionRepository.UpdateTransaction(transaction);
        await unitOfWork.CommitAsync();
        return Result.Success();
    }
}