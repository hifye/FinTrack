using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Exceptions;

/// <summary>
/// Manipulador de exceções específico para erros de validação (<see cref="ValidationException"/>).
/// </summary>
public class ValidationExceptionHandler : IExceptionHandler
{
    /// <summary>
    /// Tenta tratar a exceção de validação de forma assíncrona, retornando os detalhes do erro no formato <see cref="ValidationProblemDetails"/>.
    /// </summary>
    /// <param name="httpContext">O contexto HTTP da requisição atual.</param>
    /// <param name="exception">A exceção que foi lançada.</param>
    /// <param name="cancellationToken">Token de cancelamento para a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. Retorna true se a exceção for do tipo <see cref="ValidationException"/> e for tratada; caso contrário, false.</returns>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
            return false;

        var errors = validationException.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray()
            );

        var problem = new ValidationProblemDetails(errors)
        {
            Title = "Validation Error",
            Status = StatusCodes.Status400BadRequest,
            Type = "https://httpstatuses.com/400"
        };
        
        httpContext.Response.StatusCode = problem.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        
        return true;
    }
}