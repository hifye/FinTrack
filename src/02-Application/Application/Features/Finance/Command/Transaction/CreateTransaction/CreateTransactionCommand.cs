using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.Transaction.CreateTransaction;

public record CreateTransactionCommand(Guid AccountId, Guid CategoryId, Guid RecurringId,
    decimal Amount, string Type, string Description) : IRequest<Result<Guid>>;