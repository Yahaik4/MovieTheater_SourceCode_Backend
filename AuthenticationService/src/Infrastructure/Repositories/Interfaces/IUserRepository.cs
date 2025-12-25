using AuthenticationService.Infrastructure.EF.Models;

namespace AuthenticationService.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(Guid userId);
        Task<User> GetUserByEmail(string email);
        Task<User?> GetUserByEmailIncludingUnverified(string email);
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(User user);
        Task<bool> DeleteUser(User user);
        Task<bool> RemoveUser(User user);
        Task<List<User>> GetUsers(Guid? userId, string? role);
        Task<User> GetNotRegisterdUserByEmail(string email);
    }
}
