using Application.Abstraction.Queries;
using Application.Features.Finance.ListItem;
using Application.Interfaces.Services;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Queries.Account.GetAccountsByUserId;

public class GetAccountsByUserIdQueryHandler(IAccountQueries accountQueries, ICurrentUserService currentUser)
    : IRequestHandler<GetAccountsByUserIdQuery, Result<IReadOnlyList<AccountListItem>>>
{
    public async Task<Result<IReadOnlyList<AccountListItem>>> Handle(GetAccountsByUserIdQuery query, CancellationToken cancellationToken)
    {
        var account = await accountQueries.GetAccountsByUserId(currentUser.UserId);
        if(!account.Any())
            return Result<IReadOnlyList<AccountListItem>>.Failure("No accounts found for the user");
        
        return Result<IReadOnlyList<AccountListItem>>.Success(account);
    }
}