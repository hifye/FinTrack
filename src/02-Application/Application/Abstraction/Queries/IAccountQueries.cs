using Application.Features.Finance.ListItem;

namespace Application.Abstraction.Queries;

public interface IAccountQueries
{
    Task<AccountListItem> GetAccountDetails(Guid id);
    Task<IReadOnlyList<AccountListItem>> GetAccountsByUserId(Guid userId);
}