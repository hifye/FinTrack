using System.Data;
using Application.Abstraction.Queries;
using Application.Features.Finance.ListItem;
using Dapper;
using Infrastructure.Data.Sql;

namespace Infrastructure.Persistance.Queries;

public class TransactionQueries(IDbConnection connection) : ITransactionQueries
{
    public async Task<TransactionListItem> GetTransactionDetails(Guid id) =>
        (
            await connection.QueryFirstOrDefaultAsync<TransactionListItem>(
                TransactionSql.GetTransactionDetails,
                new { Id = id }
            )
        )!;

    public async Task<IReadOnlyList<TransactionListItem>> GetTransactionsByUserId(Guid userId) =>
        (IReadOnlyList<TransactionListItem>)await connection.QueryAsync<TransactionListItem>(
            TransactionSql.GetTransactionsByUserId,
            new { UserId = userId }
        );

    public async Task<TransactionSummary> GetTransactionSummary(
        Guid userId,
        DateTime startDate,
        DateTime endDate
    ) =>
        (
            await connection.QueryFirstOrDefaultAsync<TransactionSummary>(
                TransactionSql.GetTransactionSummary,
                new
                {
                    UserId = userId,
                    StartDate = startDate,
                    EndDate = endDate
                }
            )
        )!;
}
