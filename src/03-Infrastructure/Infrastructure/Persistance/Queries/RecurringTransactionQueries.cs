using System.Data;
using Application.Abstraction.Queries;
using Application.Features.Finance.ListItem;
using Dapper;
using Infrastructure.Data.Sql;

namespace Infrastructure.Persistance.Queries;

public class RecurringTransactionQueries(IDbConnection connection) : IRecurringTransactionQueries
{
    public async Task<RecurringTransactionListItem> GetRecurringTransactionDetails(Guid id) =>
        (
            await connection.QueryFirstOrDefaultAsync<RecurringTransactionListItem>(
                RecurringTransactionSql.GetRecurringTransactionDetails,
                new { Id = id }
            )
        )!;

    public async Task<IReadOnlyList<RecurringTransactionListItem>> GetRecurringTransactionsByUserId(Guid userId) =>
        (IReadOnlyList<RecurringTransactionListItem>)await connection.QueryAsync<RecurringTransactionListItem>(
            RecurringTransactionSql.GetRecurringTransactionsByUserId,
            new { UserId = userId }
        );
}
