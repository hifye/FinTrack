namespace Domain.Common;

public enum ErrorType
{
    None,
    NotFound,
    Validation,
    Conflict,
    Unauthorized,
    Forbidden,
    Unexpected
}