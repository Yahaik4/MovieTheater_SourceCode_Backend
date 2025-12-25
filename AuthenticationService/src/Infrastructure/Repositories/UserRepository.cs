using AuthenticationService.Data;
using AuthenticationService.Infrastructure.EF.Models;
using AuthenticationService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Infrastructure.Repositories
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
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsVerified == true && u.IsDeleted == false);
        }

        public async Task<User?> GetUserByEmailIncludingUnverified(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsDeleted == false);
        }

        public async Task<User> UpdateUser(User user)
        {
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetUsers(Guid? userId, string? role)
        {
            var query = _context.Users.AsQueryable();

            if (userId.HasValue)
                query = query.Where(x => x.Id == userId.Value);

            if (!string.IsNullOrWhiteSpace(role))
                query = query.Where(x => x.Role == role);

            query = query.Where(x => !x.IsDeleted);

            return await query.ToListAsync();
        }

        public async Task<User> GetNotRegisterdUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email &&  u.IsDeleted == false);
        }
    }
}
