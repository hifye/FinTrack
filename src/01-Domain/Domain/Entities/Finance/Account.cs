using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities.Finance;

public class Account
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Type { get; private set; } = null!;
    public Price InitialBalance { get; private set; } = null!;
    public Price CurrenBalance { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Account(Guid userId, string name, string type, Price initialBalance)
    {
        UserId = userId;
        Name = name;
        Type = type;
        InitialBalance = initialBalance;
    }
    
    public static Result<Account> Create(Guid userId, string name, string type, decimal initialBalance)
    {
        return Guard.AgainstOutOfRange(userId == Guid.Empty, "The field User id is mandatory.")
            .Bind(() => Guard.AgainstNullOrWhiteSpace(name, "The field name is mandatory."))
            .Bind(() => name.Length > 50
                ? Result.Failure("The name cannot be longer than 50 characters.")
                : Result.Success())
            .Bind(() => Guard.AgainstNullOrWhiteSpace(type, "The field type is mandatory."))
            .Bind(() => type.Length > 20 ? Result.Failure("The type cannot be longer than 20 characters.") : Result.Success())
            .Bind(() => Price.Create(initialBalance))
            .Map(validPrice => new Account(userId, name, type, validPrice));
    }

    protected Account()
    {
    }
}