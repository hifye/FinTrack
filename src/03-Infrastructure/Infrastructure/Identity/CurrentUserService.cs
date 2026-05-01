using System.Security.Claims;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Identity;

public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    public Guid UserId => Guid.Parse(accessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                                     throw new Exception("User ID claim not found"));
}