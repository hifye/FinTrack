using Domain.Entities.Auth;

namespace Application.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
}