namespace Domain.Common;

/// <summary>
/// Representa o resultado de uma operação, indicando sucesso ou falha.
/// </summary>
public class Result(bool isSuccess, string? error)
{
    /// <summary>
    /// Indica se a operação foi bem-sucedida.
    /// </summary>
    public bool IsSuccess { get; } = isSuccess;

    /// <summary>
    /// Indica se a operação falhou.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Contém a mensagem de erro em caso de falha.
    /// </summary>
    public string? Error { get; } = error;
    
    /// <summary>
    /// Retorna um resultado de sucesso.
    /// </summary>
    public static Result Success() => new Result(true, null);

    /// <summary>
    /// Cria um resultado de falha com uma mensagem específica.
    /// </summary>
    /// <param name="error">A mensagem de erro.</param>
    /// <returns>Uma nova instância de <see cref="Result"/> com falha.</returns>
    public static Result Failure(string error) => new Result(false, error);
    
    /// <summary>
    /// Encadeia uma operação se o resultado atual for sucesso.
    /// </summary>
    public Result Bind(Func<Result> func)
    {
        if (IsSuccess)
            return this;
        return func();
    }

    /// <summary>
    /// Encadeia uma operação que retorna um valor se o resultado atual for sucesso.
    /// </summary>
    public Result<T> Bind<T>(Func<Result<T>> func)
    {
        if(IsFailure)
            return Result<T>.Failure(Error!);
        return func();
    }

    /// <summary>
    /// Transforma o resultado atual em um novo tipo se for sucesso.
    /// </summary>
    public Result<T> Map<T>(Func<T> func)
    {
        if (IsFailure)
            return Result<T>.Failure(Error!);
        return Result<T>.Success(func());
    }

    /// <summary>
    /// Tenta executar uma ação e retorna sucesso ou falha em caso de exceção.
    /// </summary>
    public static Result Try(Action action, string errorMessage)
    {
        try { action(); return Result.Success(); }
        catch { return Result.Failure(errorMessage); }
    }

    public async Task<Result> BindAsync(Func<Task<Result>> func)
    {
        if (IsFailure)
            return this;
        return await func();
    }
    
    public async Task<Result<T>> BindAsync<T>(Func<Task<Result<T>>> func)
    {
        if (IsFailure)
            return Result<T>.Failure(Error!);
        return await func();
    }
}

/// <summary>
/// Representa o resultado de uma operação que retorna um valor de tipo <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">O tipo do valor de retorno.</typeparam>
public class Result<T> : Result
{
    /// <summary>
    /// O valor retornado pela operação em caso de sucesso.
    /// </summary>
    public T? Value { get; }

    public Result(bool isSuccess, string? error, T? value) : base(isSuccess, error)
    {
        Value = value;
    }
    
    /// <summary>
    /// Cria um resultado de sucesso contendo um valor.
    /// </summary>
    public static Result<T> Success(T value) => new(true, null, value);

    /// <summary>
    /// Cria um resultado de falha para um tipo específico.
    /// </summary>
    public static new Result<T> Failure(string error) => new(false, error, default);

    /// <summary>
    /// Encadeia uma operação que recebe o valor atual se for sucesso.
    /// </summary>
    public Result<K> Bind<K>(Func<T, Result<K>> func)
    {
        if (IsFailure)
            return Result<K>.Failure(Error!);
        return func(Value!);
    }

    /// <summary>
    /// Transforma o valor atual em um novo tipo se for sucesso.
    /// </summary>
    public Result<K> Map<K>(Func<T, K> func)
    {
        if (IsFailure)
            return Result<K>.Failure(Error!);
        return Result<K>.Success(func(Value!));
    }

    public async Task<Result<K>> BindAsync<K>(Func<T, Task<Result<K>>> func)
    {
        if(IsFailure)
            return Result<K>.Failure(Error!);
        return await func(Value!);
    }
    
    public async Task<Result> BindAsync(Func<T, Task<Result>> func)
    {
        if (IsFailure)
            return Result.Failure(Error!);
        return await func(Value!);
    }
}