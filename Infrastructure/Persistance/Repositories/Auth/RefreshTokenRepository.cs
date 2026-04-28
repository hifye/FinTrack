using System.Data;
using Application.Abstraction.Persistance.Repositories.Auth;
using Application.Interfaces.UnitOfWork;
using Dapper;
using Domain.Entities.Auth;
using Infrastructure.Data.Sql;

namespace Infrastructure.Persistance.Repositories.Auth;

public class RefreshTokenRepository(IUnitOfWork unitOfWork, IDbConnection connection)
    : IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetRefreshToken(
        string token,
        CancellationToken cancellationToken = default
    ) =>
        await connection.QueryFirstOrDefaultAsync<RefreshToken>(
            RefreshTokenSql.GetRefreshToken,
            new { Token = token }
        );

    public async Task CreateRefreshToken(
        RefreshToken refreshToken,
        CancellationToken cancellationToken = default
    ) =>
        await connection.ExecuteAsync(
            RefreshTokenSql.CreateRefreshToken,
            new { refreshToken.UserId, refreshToken.Token },
            unitOfWork.Transaction
        );

    public async Task<bool> RevokeRefreshToken(
        Guid id,
        CancellationToken cancellationToken = default
    ) =>
        await connection.ExecuteAsync(
            RefreshTokenSql.RevokeRefreshToken,
            new { Id = id },
            unitOfWork.Transaction
        ) > 0;
}
