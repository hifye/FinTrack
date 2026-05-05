using Application.Abstraction.Queries;
using Application.Features.Finance.ListItem;
using Application.Interfaces.Services;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Queries.Transaction.GetTransactionSummary;

public class GetTransactionSummaryQueryHandler(
    ITransactionQueries transactionQueries,
    ICurrentUserService currentUser
) : IRequestHandler<GetTransactionSummaryQuery, Result<TransactionSummary>>
{
    public async Task<Result<TransactionSummary>> Handle(GetTransactionSummaryQuery query, CancellationToken cancellationToken)
    {
        if (query.StartDate > query.EndDate)
            return Result<TransactionSummary>.Failure("Start date cannot be after end date");

        var summary = await transactionQueries.GetTransactionSummary(
            currentUser.UserId,
            query.StartDate,
            query.EndDate
        );
        
        return Result<TransactionSummary>.Success(summary);
    }
}
