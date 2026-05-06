using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Extensions;

public static class ResultExtension
{
    public static ActionResult ToActionResult(this Result result, int successStatusCode = 200)
    {
        if (result.IsSuccess)
            return new StatusCodeResult(successStatusCode);

        return result.ErrorType switch
        {
            ErrorType.NotFound => new NotFoundObjectResult(result.Error),
            ErrorType.Validation => new BadRequestObjectResult(result.Error),
            ErrorType.Conflict => new ConflictObjectResult(result.Error),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(result.Error),
            ErrorType.Forbidden => new ObjectResult(result.Error) { StatusCode = 403 },
            _ => new ObjectResult(result.Error) { StatusCode = 500 }
        };
    }

    public static ActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);

        return result.ErrorType switch
        {
            ErrorType.NotFound => new NotFoundObjectResult(result.Error),
            ErrorType.Validation => new BadRequestObjectResult(result.Error),
            ErrorType.Conflict => new ConflictObjectResult(result.Error),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(result.Error),
            _ => new ObjectResult(result.Error) { StatusCode = 500 }
        };
    }
}