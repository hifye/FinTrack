using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.Transaction.DeleteTransaction;

public record DeleteTransactionCommand(Guid Id) : IRequest<Result>;