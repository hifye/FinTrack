using Application.Abstraction.Persistance.Repositories.Catalog;
using Application.Abstraction.Persistance.Repositories.Finance;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWork;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Finance.Command.RecurringTransaction.CreateRecurringTransaction;

public class CreateRecurringTransactionCommandHandler(
    IRecurringTransactionRepository recurringTransactionRepository,
    IUnitOfWork unitOfWork,
    ICategoryRepository categoryRepository,
    IAccountRepository accountRepository,
    ICurrentUserService currentUser, ILogger<CreateRecurringTransactionCommandHandler> logger)
    : IRequestHandler<CreateRecurringTransactionCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateRecurringTransactionCommand command, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetAccountById(command.AccountId);
        if (account is null)
        {
            logger.LogWarning("Account with ID {AccountId} not found", command.AccountId);
            return Result<Guid>.Failure("Account not found", ErrorType.NotFound);
        }
            
        
        var category = await categoryRepository.GetCategoryById(command.CategoryId);
        if (category is null)
        {
            logger.LogWarning("Category with ID {CategoryId} not found", command.CategoryId);
            return Result<Guid>.Failure("Category Not Found", ErrorType.NotFound);
        }
        
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
                return Result<Guid>.Success(recurringTransaction.Id);
            });
    }
}