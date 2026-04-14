namespace Domain.Commom;

public class Result(bool isSuccess, string? error)
{
    public bool IsSuccess { get; } = isSuccess;
    public bool IsFailure => !IsSuccess;
    public string? Error { get; } = error;
    
    public static Result Success => new Result(true, null);
    public static Result Failure(string error) => new Result(false, error);
    
    public Result Bind(Func<Result> func)
    {
        if (IsSuccess)
            return this;
        return func();
    }

    public Result<T> Bind<T>(Func<Result<T>> func)
    {
        if(IsFailure)
            return Result<T>.Failure(Error!);
        return func();
    }

    public Result<T> Map<T>(Func<T> func)
    {
        if (IsFailure)
            return Result<T>.Failure(Error!);
        return Result<T>.Success(func());
    }

    public static Result Try(Action action, string errorMessage)
    {
        try { action(); return Result.Success; }
        catch { return Result.Failure(errorMessage); }
    }
}

public class Result<T> : Result
{
    public T? Value { get; }

    public Result(bool isSuccess, string? error, T? value) : base(isSuccess, error)
    {
        Value = value;
    }
    
    public static Result<T> Success(T value) => new(true, null, value);
    public static Result<T> Failure(string error) => new(false, error, default);

    public Result<K> Bind<K>(Func<T, Result<K>> func)
    {
        if (IsFailure)
            return Result<K>.Failure(Error!);
        return func(Value!);
    }

    public Result<K> Map<K>(Func<T, K> func)
    {
        if (IsFailure)
            return Result<K>.Failure(Error!);
        return Result<K>.Success(func(Value!));
    }
}