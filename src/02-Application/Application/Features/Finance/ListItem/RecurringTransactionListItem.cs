namespace Application.Features.Finance.ListItem;

public record RecurringTransactionListItem(
    Guid Id,
    Guid AccountId,
    Guid CategoryId,
    decimal Amount,
    string Frequency,
    DateTime NextOccurrence,
    DateTime CreatedAt,
    bool IsActive
);
