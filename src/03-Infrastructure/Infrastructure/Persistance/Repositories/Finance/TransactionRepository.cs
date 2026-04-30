using System.Data;
using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.UnitOfWork;
using Dapper;
using Domain.Entities.Finance;
using Infrastructure.Data.Sql;

namespace Infrastructure.Persistance.Repositories.Finance;

public class TransactionRepository(IUnitOfWork unitOfWork, IDbConnection connection)
    : ITransactionRepository
{
    public async Task<Transaction?> GetTransactionById(Guid id) =>
        await connection.QueryFirstOrDefaultAsync<Transaction>(
            TransactionSql.GetTransactionById,
            new { Id = id }
        );

    public async Task CreateTransaction(Transaction transaction) =>
        await connection.ExecuteAsync(
            TransactionSql.CreateTransaction,
            new
            {
                transaction.UserId,
                transaction.AccountId,
                transaction.CategoryId,
                transaction.RecurringId,
                transaction.Amount,
                transaction.Type,
                transaction.Description
            },
            unitOfWork.Transaction
        );

    public async Task<bool> UpdateTransaction(Transaction transaction) =>
        await connection.ExecuteAsync(
            TransactionSql.UpdateTransaction,
            new
            {
                transaction.Amount,
                transaction.Type,
                transaction.Description
            },
            unitOfWork.Transaction
        ) > 0;

    public async Task<bool> DeleteTransaction(Guid id) =>
        await connection.ExecuteAsync(TransactionSql.DeleteTransaction, new { Id = id }, unitOfWork.Transaction) > 0;
}
