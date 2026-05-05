using Application.Abstraction.Persistance.Repositories.Catalog;
using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.Transaction.CreateTransaction;

public class CreateTransactionCommandHandler(
    IRecurringTransactionRepository recurringTransactionRepository,
    ITransactionRepository transactionRepository,
    ICategoryRepository categoryRepository,
    IAccountRepository accountRepository,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateTransactionCommand, Result>
{
    public async Task<Result> Handle(
        CreateTransactionCommand command,
        CancellationToken cancellationToken
    )
    {
        var category = categoryRepository.GetCategoryById(command.CategoryId);
        if (category is null)
            return Result.Failure("Category not found.");

        var account = accountRepository.GetAccountById(command.AccountId);
        if (account is null)
            return Result.Failure("Account not found.");

        var recurringTransaction = recurringTransactionRepository.GetRecurringTransactionById(
            command.RecurringId
        );
        if (recurringTransaction is null)
            return Result.Failure("Recurring transaction not found.");

        return await Domain
            .Entities.Finance.Transaction.Create(
                currentUser.UserId,
                command.AccountId,
                command.CategoryId,
                command.RecurringId,
                command.Amount,
                command.Type,
                command.Description
            )
            .BindAsync(async transaction =>
            {
                await transactionRepository.CreateTransaction(transaction);
                await unitOfWork.CommitAsync();
                return Result.Success();
            });
    }
}
