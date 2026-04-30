using Domain.Common;

namespace Domain.Entities.Catalog;

public class Category
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public string Type { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Category(Guid userId, string name, string type)
    {
        UserId = userId;
        Name = name;
        Type = type;
    }
    
    public static Result<Category> Create(Guid userId, string name, string type)
    {
        return Guard.AgainstOutOfRange(userId == Guid.Empty, "The User id is mandatory.")
            .Bind(() => Guard.AgainstNullOrWhiteSpace(name, "The field name is mandatory."))
            .Bind(() => name.Length > 100
                ? Result.Failure("The field name cannot be longer than 100 characters.")
                : Result.Success)
            .Bind(() => Guard.AgainstNullOrWhiteSpace(type, "The field type is mandatory"))
            .Bind(() => type.Length > 100
                ? Result.Failure("The field type cannot be longer than 100 characters.")
                : Result.Success)
            .Map(() => new Category(userId, name, type));
    }
    
    protected Category() { }
}