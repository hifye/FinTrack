namespace Application.Features.Finance.ListItem;

public record AccountListItem(
    Guid Id,
    string Name,
    string Type,
    decimal CurrentBalance,
    bool IsActive,
    DateTime CreatedAt
);
