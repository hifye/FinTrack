namespace Domain.Common;

/// <summary>
/// Representa o resultado de uma operação, indicando sucesso ou falha.
/// </summary>
public class Result
{
    /// <summary>
    /// Indica se a operação foi bem-sucedida.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Indica se a operação falhou.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Contém a mensagem de erro em caso de falha.
    /// </summary>
    public string? Error { get; }

    /// <summary>
    /// O tipo do erro ocorrido.
    /// </summary>
    public ErrorType ErrorType { get; }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Result"/>.
    /// </summary>
    /// <param name="isSuccess">Indica se a operação foi bem-sucedida.</param>
    /// <param name="error">A mensagem de erro, se houver.</param>
    /// <param name="errorType">O tipo de erro ocorrido.</param>
    protected Result(bool isSuccess, string? error, ErrorType errorType)
    {
        IsSuccess = isSuccess;
        Error = error;
        ErrorType = errorType;
    }

    /// <summary>
    /// Retorna um resultado de sucesso.
    /// </summary>
    public static Result Success()
        => new Result(true, null, ErrorType.None);

    /// <summary>
    /// Cria um resultado de falha com uma mensagem específica.
    /// </summary>
    /// <param name="error">A mensagem de erro.</param>
    /// <returns>Uma nova instância de <see cref="Result"/> com falha.</returns>
    public static Result Failure(string error, ErrorType type)
        => new Result(false, error, type);

    /// <summary>
    /// Encadeia uma operação se o resultado atual for sucesso.
    /// </summary>
    /// <param name="func">A função que retorna o próximo resultado.</param>
    /// <returns>O resultado da função se o atual for sucesso; caso contrário, o próprio resultado de falha.</returns>
    public Result Bind(Func<Result> func)
        => IsFailure ? this : func();

    /// <summary>
    /// Encadeia uma operação que retorna um valor se o resultado atual for sucesso.
    /// </summary>
    /// <typeparam name="T">O tipo do valor no resultado retornado.</typeparam>
    /// <param name="func">A função que retorna o próximo resultado com valor.</param>
    /// <returns>O resultado da função se o atual for sucesso; caso contrário, um novo resultado de falha do tipo <typeparamref name="T"/>.</returns>
    public Result<T> Bind<T>(Func<Result<T>> func)
        => IsFailure ? Result<T>.Failure(Error!, ErrorType) : func();

    /// <summary>
    /// Transforma o resultado atual em um novo tipo se for sucesso.
    /// </summary>
    /// <typeparam name="T">O tipo do novo valor.</typeparam>
    /// <param name="func">A função de mapeamento que retorna o novo valor.</param>
    /// <returns>Um resultado de sucesso com o valor mapeado ou um resultado de falha.</returns>
    public Result<T> Map<T>(Func<T> func)
        => IsFailure ? Result<T>.Failure(Error!, ErrorType) : Result<T>.Success(func());

    /// <summary>
    /// Tenta executar uma ação e retorna sucesso ou falha em caso de exceção.
    /// </summary>
    /// <param name="action">A ação a ser executada.</param>
    /// <param name="errorMessage">A mensagem de erro a ser usada em caso de exceção.</param>
    /// <returns>Um resultado indicando o sucesso da ação ou falha com a mensagem fornecida.</returns>
    public static Result Try(Action action, string errorMessage)
    {
        try
        {
            action();
            return Success();
        }
        catch
        {
            return Failure(errorMessage, ErrorType.Unexpected);
        }
    }

    /// <summary>
    /// Encadeia uma operação assíncrona se o resultado atual for sucesso.
    /// </summary>
    /// <param name="func">A função assíncrona que retorna o próximo resultado.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona, contendo o resultado da função ou o atual.</returns>
    public async Task<Result> BindAsync(Func<Task<Result>> func)
        => IsFailure ? this : await func();

    /// <summary>
    /// Encadeia uma operação assíncrona que retorna um valor se o resultado atual for sucesso.
    /// </summary>
    /// <typeparam name="T">O tipo do valor no resultado retornado.</typeparam>
    /// <param name="func">A função assíncrona que retorna o próximo resultado com valor.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona, contendo o resultado da função ou um novo resultado de falha.</returns>
    public async Task<Result<T>> BindAsync<T>(Func<Task<Result<T>>> func)
        => IsFailure ? Result<T>.Failure(Error!, ErrorType) : await func();
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

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="isSuccess">Indica se a operação foi bem-sucedida.</param>
    /// <param name="error">A mensagem de erro, se houver.</param>
    /// <param name="errorType">O tipo de erro ocorrido.</param>
    /// <param name="value">O valor resultante da operação.</param>
    private Result(bool isSuccess, string? error, ErrorType errorType, T? value) : base(isSuccess, error, errorType)
    {
        Value = value;
    }

    /// <summary>
    /// Cria um resultado de sucesso contendo um valor.
    /// </summary>
    public static Result<T> Success(T value) 
        => new(true, null, ErrorType.None, value);

    /// <summary>
    /// Cria um resultado de falha para um tipo específico.
    /// </summary>
    public static new Result<T> Failure(string error, ErrorType errorType) 
        => new(false, error, errorType, default);

    /// <summary>
    /// Encadeia uma operação que recebe o valor atual se for sucesso.
    /// </summary>
    /// <typeparam name="K">O tipo do valor no novo resultado.</typeparam>
    /// <param name="func">A função que processa o valor atual e retorna um novo resultado.</param>
    /// <returns>O resultado da função ou uma falha.</returns>
    public Result<K> Bind<K>(Func<T, Result<K>> func)
        => IsFailure ? Result<K>.Failure(Error!, ErrorType) : func(Value!);

    /// <summary>
    /// Transforma o valor atual em um novo tipo se for sucesso.
    /// </summary>
    /// <typeparam name="K">O tipo do novo valor.</typeparam>
    /// <param name="func">A função que mapeia o valor atual para o novo tipo.</param>
    /// <returns>Um resultado com o valor mapeado ou uma falha.</returns>
    public Result<K> Map<K>(Func<T, K> func)
        => IsFailure ? Result<K>.Failure(Error!, ErrorType) : Result<K>.Success(func(Value!));

    /// <summary>
    /// Encadeia uma operação assíncrona que recebe o valor atual se for sucesso.
    /// </summary>
    /// <typeparam name="K">O tipo do valor no novo resultado.</typeparam>
    /// <param name="func">A função assíncrona que processa o valor e retorna um novo resultado.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona com o resultado da função ou falha.</returns>
    public async Task<Result<K>> BindAsync<K>(Func<T, Task<Result<K>>> func)
        => IsFailure ? Result<K>.Failure(Error!, ErrorType) : await func(Value!);

    /// <summary>
    /// Encadeia uma operação assíncrona que recebe o valor atual e retorna um resultado sem valor se for sucesso.
    /// </summary>
    /// <param name="func">A função assíncrona que processa o valor.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona com o resultado da função ou falha.</returns>
    public async Task<Result> BindAsync(Func<T, Task<Result>> func)
        => IsFailure ? Result.Failure(Error!, ErrorType) : await func(Value!);

    /// <summary>
    /// Encadeia uma operação que recebe o valor atual e retorna um resultado sem valor se for sucesso.
    /// </summary>
    /// <param name="func">A função que processa o valor.</param>
    /// <returns>O resultado da função ou falha.</returns>
    public Result Bind(Func<T, Result> func)
        => IsFailure ? Result.Failure(Error!, ErrorType) : func(Value!);
}