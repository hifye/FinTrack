using Domain.Entities.Auth;

namespace Application.Abstraction.Persistance.Repositories.Auth;

public interface IUserRepository
{   
    Task<User> GetUserById(Guid id);
    Task CreateUser(User user);
    Task<bool> DeleteUser(Guid id);
}