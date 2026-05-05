using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.RecurringTransaction.CreateRecurringTransaction;

public record CreateRecurringTransactionCommand(
    Guid AccountId,
    Guid CategoryId,
    decimal Amount,
    string Type,
    string Description,
    string Frequency
) : IRequest<Result>;
