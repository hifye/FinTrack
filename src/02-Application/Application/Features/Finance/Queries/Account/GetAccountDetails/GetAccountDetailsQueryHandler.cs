using Application.Abstraction.Queries;
using Application.Features.Finance.ListItem;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Queries.Account.GetAccountDetails;

public class GetAccountDetailsQueryHandler(IAccountQueries accountQueries)
    : IRequestHandler<GetAccountDetailsQuery, Result<AccountListItem>>
{
    public async Task<Result<AccountListItem>> Handle(GetAccountDetailsQuery query, CancellationToken cancellationToken)
    {
        var account = await accountQueries.GetAccountDetails(query.Id);
        if (query.Id != account.Id)
            return Result<AccountListItem>.Failure("Account not found");

        return Result<AccountListItem>.Success(account);
    }
}