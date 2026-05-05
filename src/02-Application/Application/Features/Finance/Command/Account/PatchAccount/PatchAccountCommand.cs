using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.Account.PatchAccount;

public record PatchAccountCommand(Guid Id, string? Type, bool? IsActive) : IRequest<Result>;