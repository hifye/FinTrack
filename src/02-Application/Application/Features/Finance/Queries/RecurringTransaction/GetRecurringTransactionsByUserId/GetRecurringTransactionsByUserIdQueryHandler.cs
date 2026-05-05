using Application.Abstraction.Queries;
using Application.Features.Finance.ListItem;
using Application.Interfaces.Services;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Queries.RecurringTransaction.GetRecurringTransactionsByUserId;

public class GetRecurringTransactionsByUserIdQueryHandler(IRecurringTransactionQueries recurringTransactionQueries, ICurrentUserService currentUser)
    : IRequestHandler<GetRecurringTransactionsByUserIdQuery, Result<IReadOnlyList<RecurringTransactionListItem>>>
{
    public async Task<Result<IReadOnlyList<RecurringTransactionListItem>>> Handle(GetRecurringTransactionsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var recurring = await recurringTransactionQueries.GetRecurringTransactionsByUserId(currentUser.UserId);
        if(!recurring.Any())
            return Result<IReadOnlyList<RecurringTransactionListItem>>.Failure("No recurring transactions found for the user");
        
        return Result<IReadOnlyList<RecurringTransactionListItem>>.Success(recurring);
    }
}