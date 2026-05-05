using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.RecurringTransaction.DeleteRecurringTransaction;

public record DeleteRecurringTransactionCommand(Guid Id) : IRequest<Result>;
