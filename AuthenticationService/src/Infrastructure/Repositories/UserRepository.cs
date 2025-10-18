using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;

        public UserRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUser(User user)
        {
            user.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUser(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUserById(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> UpdateUser(User user)
        {
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
