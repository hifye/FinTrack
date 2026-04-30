using System.Data;
using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.UnitOfWork;
using Dapper;
using Domain.Entities.Finance;
using Infrastructure.Data.Sql;

namespace Infrastructure.Persistance.Repositories.Finance;

public class RecurringTransactionRepository(IUnitOfWork unitOfWork, IDbConnection connection)
    : IRecurringTransactionRepository
{
    public async Task<RecurringTransaction?> GetRecurringTransactionById(Guid id) =>
        await connection.QueryFirstOrDefaultAsync<RecurringTransaction>(
            RecurringTransactionSql.GetRecurringTransactionById,
            new { Id = id }
        );

    public async Task CreateRecurringTransaction(RecurringTransaction recurringTransaction) =>
        await connection.ExecuteAsync(
            RecurringTransactionSql.CreateRecurringTransaction,
            new
            {
                recurringTransaction.UserId,
                recurringTransaction.AccountId,
                recurringTransaction.CategoryId,
                recurringTransaction.Amount,
                recurringTransaction.Type,
                recurringTransaction.Description,
                recurringTransaction.Frequency
            },
            unitOfWork.Transaction
        );

    public async Task<bool> UpdateRecurringTransaction(RecurringTransaction recurringTransaction) =>
        await connection.ExecuteAsync(
            RecurringTransactionSql.UpdateRecurringTransaction,
            new
            {
                recurringTransaction.Id,
                recurringTransaction.Amount,
                recurringTransaction.Type,
                recurringTransaction.Description,
                recurringTransaction.Frequency
            },
            unitOfWork.Transaction
        ) > 0;

    public async Task<bool> DeleteRecurringTransaction(Guid id) =>
        await connection.ExecuteAsync(
            RecurringTransactionSql.DeleteRecurringTransaction,
            new { Id = id },
            unitOfWork.Transaction
        ) > 0;
}
