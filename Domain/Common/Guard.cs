namespace Domain.Commom;

public static class Guard
{
    public static Result AgainstNullOrWhiteSpace(string value, string message)
    {
        if (String.IsNullOrWhiteSpace(value))
            return Result.Failure(message);

        return Result.Success;
    }

    public static Result AgainstOutOfRange(bool condition, string message)
    {
        if (condition)
            return Result.Failure(message);
        
        return Result.Success;
    }
    
    public static Result AgainstNull<T>(T? value, string message) where T : class
    {
        if (value == null)
            return Result<T>.Failure(message);
        
        return Result<T>.Success(value);
    }
}