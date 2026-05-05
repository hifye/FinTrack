using Application.Abstraction.Persistance.Repositories.Catalog;
using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.RecurringTransaction.CreateRecurringTransaction;

public class CreateRecurringTransactionCommandHandler(
    IRecurringTransactionRepository recurringTransactionRepository,
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository,
    IAccountRepository accountRepository,
    ICurrentUserService currentUser)
    : IRequestHandler<CreateRecurringTransactionCommand, Result>
{
    public async Task<Result> Handle(CreateRecurringTransactionCommand command, CancellationToken cancellationToken)
    {
        var account = accountRepository.GetAccountById(command.AccountId);
        if (account is null)
            return Result.Failure("Account not found");
        
        var category = categoryRepository.GetCategoryById(command.CategoryId);
        if (category is null)
            return Result.Failure("Category Not Found");
        
        return await Domain.Entities.Finance.RecurringTransaction
            .Create(currentUser.UserId, 
                command.AccountId, 
                command.CategoryId, 
                command.Amount, 
                command.Description, 
                command.Description, 
                command.Frequency)
            .BindAsync(async recurringTransaction =>
            {
                await recurringTransactionRepository.CreateRecurringTransaction(recurringTransaction);
                await unitOfWork.CommitAsync();
                return Result.Success();
            });
    }
}