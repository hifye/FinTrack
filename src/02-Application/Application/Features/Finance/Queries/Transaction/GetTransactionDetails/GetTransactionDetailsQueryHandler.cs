using Application.Abstraction.Queries;
using Application.Features.Finance.ListItem;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Queries.Transaction.GetTransactionDetails;

public class GetTransactionDetailsDetailsQueryHandler(ITransactionQueries transactionQueries)
    : IRequestHandler<GetTransactionDetailsQuery, Result<TransactionListItem>>
{
    public async Task<Result<TransactionListItem>> Handle(GetTransactionDetailsQuery query, CancellationToken cancellationToken)
    {
        var transaction = await transactionQueries.GetTransactionDetails(query.Id);
        if (query.Id != transaction.Id)
            return Result<TransactionListItem>.Failure("Transaction not found");

        return Result<TransactionListItem>.Success(transaction);
    }
}