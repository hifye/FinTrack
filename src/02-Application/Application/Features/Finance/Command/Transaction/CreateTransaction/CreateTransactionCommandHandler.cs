using Application.Abstraction.Persistance.Repositories.Catalog;
using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Finance.Command.Transaction.CreateTransaction;

public class CreateTransactionCommandHandler(
    IRecurringTransactionRepository recurringTransactionRepository,
    ITransactionRepository transactionRepository,
    ICategoryRepository categoryRepository,
    IAccountRepository accountRepository,
    ICurrentUserService currentUser,
    IUnitOfWork unitOfWork,
    ILogger<CreateTransactionCommandHandler> logger
) : IRequestHandler<CreateTransactionCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateTransactionCommand command,
        CancellationToken cancellationToken
    )
    {
        var category = categoryRepository.GetCategoryById(command.CategoryId);
        if (category is null)
        {
            logger.LogWarning("Category with ID {CategoryId} not found", command.CategoryId);
            return Result<Guid>.Failure("Category not found.", ErrorType.NotFound);
        }

        var account = accountRepository.GetAccountById(command.AccountId);
        if (account is null)
        {
            logger.LogWarning("Account with ID {AccountId} not found", command.AccountId);
            return Result<Guid>.Failure("Account not found.", ErrorType.NotFound);
        }

        var recurringTransaction = recurringTransactionRepository.GetRecurringTransactionById(
            command.RecurringId
        );
        if (recurringTransaction is null)
        {
            logger.LogWarning("Recurring transaction with ID {RecurringId} not found", command.RecurringId);
            return Result<Guid>.Failure("Recurring transaction not found.", ErrorType.NotFound);
        }

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
                return Result<Guid>.Success(transaction.Id);
            });
    }
}
