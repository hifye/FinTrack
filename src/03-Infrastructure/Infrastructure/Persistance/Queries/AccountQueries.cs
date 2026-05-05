using System.Data;
using Application.Abstraction.Queries;
using Application.Features.Finance.ListItem;
using Dapper;
using Infrastructure.Data.Sql;

namespace Infrastructure.Persistance.Queries;

public class AccountQueries(IDbConnection connection) : IAccountQueries
{
    public async Task<AccountListItem> GetAccountDetails(Guid id) =>
        (await connection.QueryFirstOrDefaultAsync<AccountListItem>(AccountSql.GetAccountDetails, new { Id = id }))!;

    public async Task<IReadOnlyList<AccountListItem>> GetAccountsByUserId(Guid userId) =>
        (IReadOnlyList<AccountListItem>)await connection.QueryAsync<AccountListItem>(
            AccountSql.GetAccountsByUserId,
            new { UserId = userId }
        );
}
