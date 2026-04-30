using Domain.Entities.Auth;

namespace Application.Abstraction.Persistance.Repositories.Auth;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetRefreshToken(string token, CancellationToken cancellationToken = default);
    Task CreateRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task<bool> RevokeRefreshToken(Guid id, CancellationToken cancellationToken = default);
}