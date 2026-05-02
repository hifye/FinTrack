using System.Data;
using Application.Abstraction.Persistance.Repositories.Catalog;
using Application.Interfaces.UnitOfWork;
using Dapper;
using Domain.Entities.Catalog;
using Infrastructure.Data.Sql;

namespace Infrastructure.Persistance.Repositories.Catalog;

public class CategoryRepository(IUnitOfWork unitOfWork, IDbConnection connection)
    : ICategoryRepository
{
    public async Task<Category?> GetCategoryById(Guid id) =>
        await connection.QueryFirstOrDefaultAsync<Category>(
            CategorySql.GetCategoryById,
            new { Id = id }
        );

    public Task CreateCategory(Category category) =>
        connection.ExecuteAsync(
            CategorySql.CreateCategory,
            new
            {
                category.UserId,
                category.Name,
                category.Type,
                category.IsActive,
                category.CreatedAt
            },
            unitOfWork.Transaction
        );

    public async Task<bool> UpdateCategory(Category category) =>
        await connection.ExecuteAsync(
            CategorySql.UpdateCategory,
            new
            {
                category.Id,
                category.Name,
                category.Type,
                category.IsActive
            },
            unitOfWork.Transaction
        ) > 0;

    public async Task<bool> DeleteCategory(Guid id) =>
        await connection.ExecuteAsync(
            CategorySql.DeleteCategory,
            new { Id = id },
            unitOfWork.Transaction
        ) > 0;
}
