using Application.Abstraction.Queries;
using Application.Features.Finance.ListItem;
using Application.Interfaces.Services;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Finance.Queries.Transaction.GetTransactionSummary;

public class GetTransactionSummaryQueryHandler(
    ITransactionQueries transactionQueries,
    ICurrentUserService currentUser,
    ILogger<GetTransactionSummaryQueryHandler> logger
) : IRequestHandler<GetTransactionSummaryQuery, Result<TransactionSummary>>
{
    public async Task<Result<TransactionSummary>> Handle(GetTransactionSummaryQuery query, CancellationToken cancellationToken)
    {
        if (query.StartDate > query.EndDate)
        {
            logger.LogWarning("Start date cannot be after end date");
            return Result<TransactionSummary>.Failure("Start date cannot be after end date", ErrorType.Validation);
        }

        var summary = await transactionQueries.GetTransactionSummary(
            currentUser.UserId,
            query.StartDate,
            query.EndDate
        );
        
        return Result<TransactionSummary>.Success(summary);
    }
}
