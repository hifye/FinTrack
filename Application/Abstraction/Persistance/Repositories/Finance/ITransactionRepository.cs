using System.Transactions;
using Transaction = Domain.Entities.Finance.Transaction;

namespace Application.Abstraction.Persistance.Repositories.Finance;

public interface ITransactionRepository
{
    Task<Transaction?> GetTransactionById(Guid id);
    Task CreateTransaction(Transaction transaction);
    Task<bool> UpdateTransaction(Transaction transaction);
    Task<bool> DeleteTransaction(Guid id);
}