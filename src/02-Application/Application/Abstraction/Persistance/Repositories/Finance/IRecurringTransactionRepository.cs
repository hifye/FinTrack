using Domain.Entities.Finance;

namespace Application.Abstraction.Persistance.Repositories.Finance;

public interface IRecurringTransactionRepository
{
    Task<RecurringTransaction?> GetRecurringTransactionById(Guid id);
    Task CreateRecurringTransaction(RecurringTransaction recurringTransaction);
    Task<bool> UpdateRecurringTransaction(RecurringTransaction recurringTransaction);
    Task<bool> DeleteRecurringTransaction(Guid id);
}