using Application.Abstraction.Queries;
using Application.Features.Finance.ListItem;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Finance.Queries.Account.GetAccountDetails;

public class GetAccountDetailsQueryHandler(IAccountQueries accountQueries, ILogger<GetAccountDetailsQueryHandler> logger)
    : IRequestHandler<GetAccountDetailsQuery, Result<AccountListItem>>
{
    public async Task<Result<AccountListItem>> Handle(GetAccountDetailsQuery query, CancellationToken cancellationToken)
    {
        var account = await accountQueries.GetAccountDetails(query.Id);
        
        if (account is not null) return Result<AccountListItem>.Success(account);
        logger.LogWarning("Account with ID {AccountId} not found", query.Id);
        return Result<AccountListItem>.Failure("Account not found", ErrorType.NotFound);
    }
}