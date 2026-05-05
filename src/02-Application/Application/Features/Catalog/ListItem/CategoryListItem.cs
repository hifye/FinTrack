namespace Application.Features.Catalog.ListItem;

public record CategoryListItem(
    Guid Id,
    string Name,
    string Type,
    bool IsActive,
    DateTime CreatedAt
);
