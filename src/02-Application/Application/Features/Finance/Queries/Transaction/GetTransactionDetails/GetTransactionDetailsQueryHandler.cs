using Application.Abstraction.Queries;
using Application.Features.Finance.ListItem;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Finance.Queries.Transaction.GetTransactionDetails;

public class GetTransactionDetailsDetailsQueryHandler(ITransactionQueries transactionQueries, ILogger<GetTransactionDetailsDetailsQueryHandler> logger)
    : IRequestHandler<GetTransactionDetailsQuery, Result<TransactionListItem>>
{
    public async Task<Result<TransactionListItem>> Handle(GetTransactionDetailsQuery query, CancellationToken cancellationToken)
    {
        var transaction = await transactionQueries.GetTransactionDetails(query.Id);

        if (query.Id == transaction.Id) return Result<TransactionListItem>.Success(transaction);
        logger.LogWarning("Transaction with ID {TransactionId} not found for query ID {QueryId}", transaction.Id, query.Id);
        return Result<TransactionListItem>.Failure("Transaction not found", ErrorType.NotFound);

    }
}