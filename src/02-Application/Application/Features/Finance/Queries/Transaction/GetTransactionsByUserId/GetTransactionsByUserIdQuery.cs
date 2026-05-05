using Application.Features.Finance.ListItem;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Queries.Transaction.GetTransactionsByUserId;

public record GetTransactionsByUserIdQuery : IRequest<Result<IReadOnlyList<TransactionListItem>>>;