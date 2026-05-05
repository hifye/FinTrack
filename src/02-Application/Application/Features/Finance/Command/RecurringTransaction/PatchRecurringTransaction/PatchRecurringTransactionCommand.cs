using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.RecurringTransaction.PatchRecurringTransaction;

public record PatchRecurringTransactionCommand(
    Guid Id,
    decimal? Amount,
    string? Type,
    string? Description,
    string? Frequency,
    bool? IsActive
) : IRequest<Result>;
