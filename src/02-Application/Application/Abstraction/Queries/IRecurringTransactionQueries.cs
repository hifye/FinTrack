using Application.Features.Finance.ListItem;

namespace Application.Abstraction.Queries;

public interface IRecurringTransactionQueries
{
    Task<RecurringTransactionListItem> GetRecurringTransactionDetails(Guid id);
    Task<IReadOnlyList<RecurringTransactionListItem>> GetRecurringTransactionsByUserId(Guid userId);
}