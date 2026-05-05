using Application.Abstraction.Queries;
using Application.Features.Finance.ListItem;
using Application.Interfaces.Services;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Queries.Transaction.GetTransactionsByUserId;

public class GetTransactionsByUserIdQueryHandler(ITransactionQueries transactionQueries, ICurrentUserService currentUser)
    : IRequestHandler<GetTransactionsByUserIdQuery, Result<IReadOnlyList<TransactionListItem>>>
{
    public async Task<Result<IReadOnlyList<TransactionListItem>>> Handle(GetTransactionsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await transactionQueries.GetTransactionsByUserId(currentUser.UserId);
        if(!transaction.Any())
            return Result<IReadOnlyList<TransactionListItem>>.Failure("No transactions found for the user");
        
        return Result<IReadOnlyList<TransactionListItem>>.Success(transaction);
    }
}