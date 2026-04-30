using Domain.Entities.Catalog;

namespace Application.Abstraction.Persistance.Repositories.Catalog;

public interface ICategoryRepository
{
    Task<Category?> GetCategoryById(Guid id);
    Task CreateCategory(Category category);
    Task<bool> UpdateCategory(Category category);
    Task<bool> DeleteCategory(Guid id);
}