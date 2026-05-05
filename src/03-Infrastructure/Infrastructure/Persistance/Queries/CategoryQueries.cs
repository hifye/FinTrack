using System.Data;
using Application.Abstraction.Queries;
using Application.Features.Catalog.ListItem;
using Dapper;
using Infrastructure.Data.Sql;

namespace Infrastructure.Persistance.Queries;

public class CategoryQueries(IDbConnection connection) : ICategoryQueries
{
    public async Task<CategoryListItem> GetCategoryDetails(Guid id) =>
        (
            await connection.QueryFirstOrDefaultAsync<CategoryListItem>(
                CategorySql.GetCategoryDetails,
                new { Id = id }
            )
        )!;

    public async Task<IReadOnlyList<CategoryListItem>> GetCategoriesByUserId(Guid userId) =>
        (IReadOnlyList<CategoryListItem>)await connection.QueryAsync<CategoryListItem>(
            CategorySql.GetCategoriesByUserId,
            new { UserId = userId }
        );
}
