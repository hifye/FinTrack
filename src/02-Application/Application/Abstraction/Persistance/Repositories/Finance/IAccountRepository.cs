using Domain.Entities.Finance;

namespace Application.Abstraction.Persistance.Repositories.Finance;

public interface IAccountRepository
{
    Task<Account?> GetAccountById(Guid id);
    Task CreateAccount(Account account);
    Task<bool> UpdateAccount(Account account);
    Task<bool> DeleteAccount(Guid id);
}