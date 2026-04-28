using System.Data;
using Application.Abstraction.Persistance.Repositories.Auth;
using Application.Interfaces.UnitOfWork;
using Dapper;
using Domain.Entities.Auth;
using Infrastructure.Data.Sql;

namespace Infrastructure.Persistance.Repositories.Auth;

public class UserRepository(IUnitOfWork unitOfWork, IDbConnection connection) : IUserRepository
{
    public async Task<User> GetUserById(Guid id) =>
        (await connection.QueryFirstOrDefaultAsync<User>(UserSql.GetUserById, new { Id = id }))!;

    public async Task CreateUser(User user) =>
        await connection.ExecuteAsync(
            UserSql.CreateUser,
            new
            {
                user.Name,
                user.Email,
                user.PasswordHash,
            },
            unitOfWork.Transaction
        );

    public async Task<bool> DeleteUser(Guid id) =>
        await connection.ExecuteAsync(UserSql.DeleteUser, new { Id = id }, unitOfWork.Transaction)
        > 0;
}
