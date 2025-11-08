using src.Infrastructure.EF.Models;

namespace src.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(Guid userId);
        Task<User> GetUserByEmail(string email);
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(User user);
        Task<bool> DeleteUser(User user);
        Task<bool> RemoveUser(User user);
        Task<bool> ChangePassword(string email, string newPassword);
        Task SavePendingChangeAsync(string email, string hashedPassword, TimeSpan expiry);
        Task<string?> GetPendingChangeAsync(string email);
        Task DeletePendingChangeAsync(string email);
    }
}
