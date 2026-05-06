namespace Application.Features.Finance.ListItem;

public record RecurringTransactionListItem(
    Guid Id,
    Guid AccountId,
    Guid CategoryId,
    decimal Amount,
    string Type,
    string Description,
    string Frequency,
    DateTime NextOccurrence,
    DateTime EndDate,
    bool IsActive
);
