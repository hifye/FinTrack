using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.Account.DeleteAccount;

public record DeleteAccountCommand(Guid Id) : IRequest<Result>;
