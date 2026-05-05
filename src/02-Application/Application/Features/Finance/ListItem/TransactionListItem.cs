namespace Application.Features.Finance.ListItem;

public record TransactionListItem(
    Guid Id,
    Guid AccountId,
    Guid CategoryId,
    Guid RecurringId,
    decimal Amount,
    string Type,
    string Description,
    DateTime CreatedAt,
    DateTime TransactionDate,
    DateTime UpdatedAt
);
