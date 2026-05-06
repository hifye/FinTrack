using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities.Finance;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }
    public Guid RecurringId { get; private set; }
    public Price Amount { get; private set; } = null!;
    public string Type { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTime TransactionDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    private Transaction(Guid userId, Guid accountId, Guid categoryId, Guid recurringId, Price amount, string type, string description)
    {
        UserId = userId;
        AccountId = accountId;
        CategoryId = categoryId;
        RecurringId = recurringId;
        Amount = amount;
        Type = type;
        Description = description;
        TransactionDate = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<Transaction> Create(Guid userId, Guid accountId, Guid categoryId, Guid recurringId,
        decimal amount, string type, string description)
    {
        return Guard.AgainstOutOfRange(userId == Guid.Empty, "The field User id cannot be empty")
            .Bind(() => Guard.AgainstOutOfRange(accountId == Guid.Empty, "The field Account id cannot be empty"))
            .Bind(() => Guard.AgainstOutOfRange(categoryId == Guid.Empty, "The field Category id cannot be empty"))
            .Bind(() => Guard.AgainstOutOfRange(recurringId == Guid.Empty, "The field Recurring id cannot be empty"))
            .Bind(() => Guard.AgainstNullOrWhiteSpace(type, "The field Type is mandatory"))
            .Bind(() => type.Length > 100
                ? Result.Failure("The field Type cannot be longer than 100 characters.", ErrorType.Validation)
                : Result.Success())
            .Bind(() => description.Length > 250
                ? Result.Failure("The field Description cannot be longer than 250 characters.", ErrorType.Validation)
                : Result.Success())
            .Bind(() => Price.Create(amount))
            .Map(validPrice =>
                new Transaction(userId, accountId, categoryId, recurringId, validPrice, type, description));
    }

    public Result Patch(string? type, string? description)
    {
        return Guard.AgainstOutOfRange(type == null && description == null, "At least one field must be provided for patching.")
            .Bind(() => type != null ? UpdateType(type) : Result.Success())
            .Bind(() => description != null ? UpdateDescription(description) : Result.Success())
            .Map(() => UpdatedAt = DateTime.UtcNow)
            .Bind(Result.Success);
    }

    private Result UpdateType(string type)
    {
        return Guard.AgainstOutOfRange(type.Length > 100, "The field type cannot be longer than 100 characters.")
            .Bind(() =>
            {
                Type = type;
                return Result.Success();
            });
    }

    private Result UpdateDescription(string description)
    {
        return Guard.AgainstOutOfRange(description.Length > 250, "The field Description cannot be longer than 250 characters.")
            .Bind(() =>
            {
                Description = description;
                return Result.Success();
            });   
    }
    
    protected Transaction()
    {
    }
}