using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.Transaction.PatchTransaction;

public record PatchTransactionCommand(Guid Id, string? Type, string? Description) : IRequest<Result>;