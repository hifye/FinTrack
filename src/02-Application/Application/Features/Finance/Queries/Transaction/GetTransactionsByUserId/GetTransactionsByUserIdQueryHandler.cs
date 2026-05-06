using Application.Abstraction.Queries;
using Application.Features.Finance.ListItem;
using Application.Interfaces.Services;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Finance.Queries.Transaction.GetTransactionsByUserId;

public class GetTransactionsByUserIdQueryHandler(ITransactionQueries transactionQueries, ICurrentUserService currentUser, ILogger<GetTransactionsByUserIdQueryHandler> logger)
    : IRequestHandler<GetTransactionsByUserIdQuery, Result<IReadOnlyList<TransactionListItem>>>
{
    public async Task<Result<IReadOnlyList<TransactionListItem>>> Handle(GetTransactionsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await transactionQueries.GetTransactionsByUserId(currentUser.UserId);

        if (transaction.Any()) return Result<IReadOnlyList<TransactionListItem>>.Success(transaction);
        logger.LogWarning("No transactions found for user with ID {UserId}", currentUser.UserId);
        return Result<IReadOnlyList<TransactionListItem>>.Failure("No transactions found for the user", ErrorType.NotFound);

    }
}