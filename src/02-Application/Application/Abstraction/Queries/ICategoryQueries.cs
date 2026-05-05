using Application.Features.Catalog.ListItem;

namespace Application.Abstraction.Queries;

public interface ICategoryQueries
{
    Task<CategoryListItem> GetCategoryDetails(Guid id);
    Task<IReadOnlyList<CategoryListItem>> GetCategoriesByUserId(Guid userId);
}