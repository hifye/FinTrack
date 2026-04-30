using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities.Auth;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    private User(string name, Email email, string passwordHash)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
    }
    
    public static Result<User> Create(string name, string email, string passwordHash)
    {
        return Guard
            .AgainstNullOrWhiteSpace(name, "Name cannot be empty.")
            .Bind(() => name.Length > 200
                ? Result.Failure("Name cannot be longer than 200 characters.")
                : Result.Success)
            .Bind(() => Guard.AgainstNullOrWhiteSpace(passwordHash, "Password cannot be empty."))
            .Bind(() => Email.Create(email))
            .Map(validEmail => new User(name, validEmail, passwordHash));
    }
    
    protected User() { }
}