using System.Data;
using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.UnitOfWork;
using Dapper;
using Domain.Entities.Finance;
using Infrastructure.Data.Sql;

namespace Infrastructure.Persistance.Repositories.Finance;

public class AccountRepository(IUnitOfWork unitOfWork, IDbConnection connection)
    : IAccountRepository
{
    public async Task<Account?> GetAccountById(Guid id) =>
        await connection.QueryFirstOrDefaultAsync<Account>(
            AccountSql.GetAccountById,
            new { Id = id }
        );

    public Task CreateAccount(Account account) =>
        connection.ExecuteAsync(
            AccountSql.CreateAccount,
            new
            {
                account.UserId,
                account.Name,
                account.Type,
                account.InitialBalance,
            },
            unitOfWork.Transaction
        );

    public async Task<bool> UpdateAccount(Account account) =>
        await connection.ExecuteAsync(
            AccountSql.UpdateAccount,
            new
            {
                account.Id,
                account.Name,
                account.Type,
                account.IsActive,
            },
            unitOfWork.Transaction
        ) > 0;

    public async Task<bool> DeleteAccount(Guid id) =>
        await connection.ExecuteAsync(
            AccountSql.DeleteAccount,
            new { Id = id },
            unitOfWork.Transaction
        ) > 0;
}
