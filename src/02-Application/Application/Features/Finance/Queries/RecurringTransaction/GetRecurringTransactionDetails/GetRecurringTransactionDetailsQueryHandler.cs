using Application.Abstraction.Queries;
using Application.Features.Finance.ListItem;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Finance.Queries.RecurringTransaction.GetRecurringTransactionDetails;

public class GetRecurringTransactionDetailsQueryHandler(IRecurringTransactionQueries recurringTransactionQueries, ILogger<GetRecurringTransactionDetailsQueryHandler> logger)
    : IRequestHandler<GetRecurringTransactionDetailsQuery, Result<RecurringTransactionListItem>>
{
    public async Task<Result<RecurringTransactionListItem>> Handle(GetRecurringTransactionDetailsQuery query, CancellationToken cancellationToken)
    {
        var recurring = await recurringTransactionQueries.GetRecurringTransactionDetails(query.Id);
        
        if (recurring is not null) return Result<RecurringTransactionListItem>.Success(recurring);
        logger.LogWarning("Recurring Transaction with ID {RecurringId} not found", query.Id);
        return Result<RecurringTransactionListItem>.Failure("Recurring Transaction not found", ErrorType.NotFound);
    }
}