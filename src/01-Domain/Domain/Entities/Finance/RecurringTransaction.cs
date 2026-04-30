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
    public DateTime NextOccurence { get; private set; }
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
    }

    public static Result<RecurringTransaction> Create(Guid userId, Guid accountId, Guid categoryId, decimal amount, string type,
        string description, string frequency)
    {
        return Guard.AgainstOutOfRange(userId == Guid.Empty, "The field User id cannot be empty.")
            .Bind(() => Guard.AgainstOutOfRange(accountId == Guid.Empty, "The field Account id cannot be empty."))
            .Bind(() => Guard.AgainstOutOfRange(categoryId == Guid.Empty, "The field Category id cannot be empty."))
            .Bind(() => Guard.AgainstNullOrWhiteSpace(type, "The field Type is mandatory."))
            .Bind(() => type.Length > 100
                ? Result.Failure("The field Type cannot be longer than 100 characters.")
                : Result.Success())
            .Bind(() => description.Length > 250
                ? Result.Failure("The field Description cannot be longer than 250 characters.")
                : Result.Success())
            .Bind(() => Guard.AgainstNullOrWhiteSpace(frequency, "The field Frequency is mandatory."))
            .Bind(() => frequency.Length > 50
                ? Result.Failure("The field Frequency cannot be longer than 50 characters.")
                : Result.Success())
            .Bind(() => Price.Create(amount))
            .Map(validAmount =>
                new RecurringTransaction(userId, accountId, categoryId, validAmount, type, description, frequency));
    }

    protected RecurringTransaction()
    {
    }
}