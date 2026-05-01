using Domain.Entities.Auth;
using Domain.ValueObjects;

namespace Application.Abstraction.Persistance.Repositories.Auth;

public interface IUserRepository
{   
    Task<User> GetUserById(Guid id);
    Task<User?> GetUserByEmail(Email email);
    Task CreateUser(User user);
    Task<bool> DeleteUser(Guid id);
}