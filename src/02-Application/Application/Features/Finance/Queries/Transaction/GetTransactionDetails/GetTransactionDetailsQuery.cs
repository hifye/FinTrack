using Application.Features.Finance.ListItem;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Queries.Transaction.GetTransactionDetails;

public record GetTransactionDetailsQuery(Guid Id) : IRequest<Result<TransactionListItem>>;