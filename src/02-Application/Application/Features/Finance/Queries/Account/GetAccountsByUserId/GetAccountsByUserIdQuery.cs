using Application.Features.Finance.ListItem;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Queries.Account.GetAccountsByUserId;

public record GetAccountsByUserIdQuery : IRequest<Result<IReadOnlyList<AccountListItem>>>;