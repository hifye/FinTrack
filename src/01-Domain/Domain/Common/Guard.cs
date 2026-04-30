namespace Domain.Common;

/// <summary>
/// Fornece métodos utilitários para validações de proteção (Guard clauses).
/// </summary>
public static class Guard
{
    /// <summary>
    /// Garante que uma string não seja nula ou composta apenas por espaços em branco.
    /// </summary>
    /// <param name="value">O valor a ser verificado.</param>
    /// <param name="message">A mensagem de erro caso a validação falhe.</param>
    /// <returns>Um <see cref="Result"/> indicando sucesso ou falha.</returns>
    public static Result AgainstNullOrWhiteSpace(string value, string message)
    {
        if (String.IsNullOrWhiteSpace(value))
            return Result.Failure(message);

        return Result.Success;
    }

    /// <summary>
    /// Verifica se uma condição de "fora de intervalo" ou inválida é verdadeira.
    /// </summary>
    /// <param name="condition">A condição que, se verdadeira, indica uma falha.</param>
    /// <param name="message">A mensagem de erro caso a condição seja verdadeira.</param>
    /// <returns>Um <see cref="Result"/> indicando sucesso ou falha.</returns>
    public static Result AgainstOutOfRange(bool condition, string message)
    {
        if (condition)
            return Result.Failure(message);
        
        return Result.Success;
    }
    
    /// <summary>
    /// Garante que um objeto não seja nulo.
    /// </summary>
    /// <typeparam name="T">O tipo do objeto.</typeparam>
    /// <param name="value">O valor a ser verificado.</param>
    /// <param name="message">A mensagem de erro caso o valor seja nulo.</param>
    /// <returns>Um <see cref="Result{T}"/> contendo o valor em caso de sucesso ou a falha.</returns>
    public static Result AgainstNull<T>(T? value, string message) where T : class
    {
        if (value == null)
            return Result<T>.Failure(message);
        
        return Result<T>.Success(value);
    }
}