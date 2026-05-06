using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities.Finance;

public class RecurringTransaction
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }
    public Price Amount { get; private set; } = null!;
    public string Type { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Frequency { get; private set; } = null!;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public DateTime NextOccurrence { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private RecurringTransaction(Guid userId, Guid accountId, Guid categoryId, Price amount, string type,
        string description, string frequency)
    {
        UserId = userId;
        AccountId = accountId;
        CategoryId = categoryId;
        Amount = amount;
        Type = type;
        Description = description;
        Frequency = frequency;
        NextOccurrence = DateTime.UtcNow.AddDays(30);
        StartDate = DateTime.UtcNow;
        EndDate = NextOccurrence;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<RecurringTransaction> Create(Guid userId, Guid accountId, Guid categoryId, decimal amount, string type,
        string description, string frequency)
    {
        return Guard.AgainstOutOfRange(userId == Guid.Empty, "The field User id cannot be empty.")
            .Bind(() => Guard.AgainstOutOfRange(accountId == Guid.Empty, "The field Account id cannot be empty."))
            .Bind(() => Guard.AgainstOutOfRange(categoryId == Guid.Empty, "The field Category id cannot be empty."))
            .Bind(() => Guard.AgainstNullOrWhiteSpace(type, "The field Type is mandatory."))
            .Bind(() => type.Length > 100
                ? Result.Failure("The field Type cannot be longer than 100 characters.", ErrorType.Validation)
                : Result.Success())
            .Bind(() => description.Length > 250
                ? Result.Failure("The field Description cannot be longer than 250 characters.", ErrorType.Validation)
                : Result.Success())
            .Bind(() => Guard.AgainstNullOrWhiteSpace(frequency, "The field Frequency is mandatory."))
            .Bind(() => frequency.Length > 50
                ? Result.Failure("The field Frequency cannot be longer than 50 characters.", ErrorType.Validation)
                : Result.Success())
            .Bind(() => Price.Create(amount))
            .Map(validAmount =>
                new RecurringTransaction(userId, accountId, categoryId, validAmount, type, description, frequency));
    }
    
    public Result Patch(decimal? amount, string? type, string? description, string? frequency, bool? isActive)
    {
        return Guard.AgainstOutOfRange(amount == null && type == null && description == null && frequency == null && isActive == null, "At least one field must be provided for patching.")
            .Bind(() => amount != null ? UpdateAmount(amount.Value) : Result.Success())
            .Bind(() => type != null ? UpdateType(type) : Result.Success())
            .Bind(() => description != null ? UpdateDescription(description) : Result.Success())
            .Bind(() => frequency != null ? UpdateFrequency(frequency) : Result.Success())
            .Bind(() => isActive != null ? UpdateIsActive(isActive.Value) : Result.Success());
    }

    private Result UpdateAmount(decimal amount)
    {
        return Price.Create(amount)
            .Bind(validAmount =>
            {
                Amount = validAmount;
                return Result.Success();
            });
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

    private Result UpdateFrequency(string frequency)
    {
        return Guard.AgainstOutOfRange(frequency.Length > 50, "The field Frequency cannot be longer than 50 characters.")
            .Bind(() =>
            {
                Frequency = frequency;
                return Result.Success();
            });
    }
    
    private Result UpdateIsActive(bool isActive)
    {
        IsActive = isActive;
        return Result.Success();
    }
    protected RecurringTransaction()
    {
    }
}