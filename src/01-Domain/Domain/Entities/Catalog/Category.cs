using Domain.Common;

namespace Domain.Entities.Catalog;

public class Category
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Type { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Category(Guid userId, string name, string type)
    {
        UserId = userId;
        Name = name;
        Type = type;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }
    
    public static Result<Category> Create(Guid userId, string name, string type)
    {
        return Guard.AgainstOutOfRange(userId == Guid.Empty, "The User id is mandatory.")
            .Bind(() => Guard.AgainstNullOrWhiteSpace(name, "The field name is mandatory."))
            .Bind(() => name.Length > 100
                ? Result.Failure("The field name cannot be longer than 100 characters.")
                : Result.Success())
            .Bind(() => Guard.AgainstNullOrWhiteSpace(type, "The field type is mandatory"))
            .Bind(() => type.Length > 100
                ? Result.Failure("The field type cannot be longer than 100 characters.")
                : Result.Success())
            .Map(() => new Category(userId, name, type));
    }

    public Result Patch(string? name, string? type, bool? isActive)
    {
        return Guard.AgainstOutOfRange(name == null && type == null && isActive == null, "At least one field must be provided for patching.")
            .Bind(() => name != null ? UpdateName(name) : Result.Success())
            .Bind(() => type != null ? UpdateType(type) : Result.Success())
            .Bind(() => isActive != null ? UpdateIsActive(isActive.Value) : Result.Success());
    }

    private Result UpdateName(string name)
    {
        return Guard.AgainstOutOfRange(name.Length > 100, "The field name cannot be longer than 100 characters.")
            .Bind(() =>
            {
                Name = name;
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

    private Result UpdateIsActive(bool isActive)
    {
        IsActive = isActive;
        return Result.Success();
    }
    
    protected Category() { }
}