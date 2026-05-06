using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Exceptions;

/// <summary>
/// Manipulador global de exceções para capturar e tratar erros não esperados na aplicação.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    /// <summary>
    /// Tenta tratar a exceção ocorrida de forma assíncrona, retornando uma resposta formatada como <see cref="ProblemDetails"/>.
    /// </summary>
    /// <param name="httpContext">O contexto HTTP da requisição atual.</param>
    /// <param name="exception">A exceção que foi lançada.</param>
    /// <param name="cancellationToken">Token de cancelamento para a operação assíncrona.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona. O valor contém true se a exceção foi tratada; caso contrário, false.</returns>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problem = new ProblemDetails
        {
            Title = "Internal Server Error",
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://httpstatuses.com/500"
        };
        
        httpContext.Response.StatusCode = problem.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}