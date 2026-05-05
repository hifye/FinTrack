using Application.Abstraction.Queries;
using Application.Features.Finance.ListItem;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Queries.RecurringTransaction.GetRecurringTransactionDetails;

public class GetRecurringTransactionDetailsQueryHandler(IRecurringTransactionQueries recurringTransactionQueries)
    : IRequestHandler<GetRecurringTransactionDetailsQuery, Result<RecurringTransactionListItem>>
{
    public async Task<Result<RecurringTransactionListItem>> Handle(GetRecurringTransactionDetailsQuery query, CancellationToken cancellationToken)
    {
        var recurring = await recurringTransactionQueries.GetRecurringTransactionDetails(query.Id);
        if (query.Id != recurring.Id)
            return Result<RecurringTransactionListItem>.Failure("Recurring Transaction not found");

        return Result<RecurringTransactionListItem>.Success(recurring);
    }
}