using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.Account.CreateAccount;

public record CreateAccountCommand(string Name, string Type, decimal InitialBalance) : IRequest<Result<Guid>>;