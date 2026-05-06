using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.Account.CreateAccount;

public class CreateAccountCommandHandler(
    IAccountRepository accountRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser)
    : IRequestHandler<CreateAccountCommand, Result<Guid>>
{   
    public async Task<Result<Guid>> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        return await Domain.Entities.Finance.Account.Create(currentUser.UserId, command.Name, command.Type, command.InitialBalance)
            .BindAsync(async account =>
            {
                await accountRepository.CreateAccount(account);
                await unitOfWork.CommitAsync();
                return Result<Guid>.Success(account.Id);
            });
    }
}