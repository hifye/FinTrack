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
            return Result.Failure(message, ErrorType.Validation);

        return Result.Success();
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
            return Result.Failure(message, ErrorType.Validation);
        
        return Result.Success();
    }
}