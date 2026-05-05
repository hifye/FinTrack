using Application.Features.Finance.ListItem;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Queries.RecurringTransaction.GetRecurringTransactionsByUserId;

public record GetRecurringTransactionsByUserIdQuery : IRequest<Result<IReadOnlyList<RecurringTransactionListItem>>>;
