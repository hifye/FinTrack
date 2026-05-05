using Application.Features.Finance.ListItem;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Queries.RecurringTransaction.GetRecurringTransactionDetails;

public record GetRecurringTransactionDetailsQuery(Guid Id) : IRequest<Result<RecurringTransactionListItem>>;