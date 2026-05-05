using Application.Features.Finance.ListItem;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Queries.Account.GetAccountDetails;

public record GetAccountDetailsQuery(Guid Id) : IRequest<Result<AccountListItem>>;