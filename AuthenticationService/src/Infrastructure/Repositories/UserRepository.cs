using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Exceptions;
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

        public async Task<bool> ChangePassword(string email, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.Password = newPassword;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task SavePendingChangeAsync(string email, string hashedPassword, TimeSpan expiry)
        {
            var existing = await _context.Set<PendingPasswordChange>()
                .FirstOrDefaultAsync(x => x.Email == email);

            if (existing != null)
                _context.Remove(existing);

            var pending = new PendingPasswordChange
            {
                Id = Guid.NewGuid(),
                Email = email,
                HashedNewPassword = hashedPassword,
                ExpiryAt = DateTime.UtcNow.Add(expiry)
            };

            await _context.AddAsync(pending);
            await _context.SaveChangesAsync();
        }

        public async Task<string?> GetPendingChangeAsync(string email)
        {
            var record = await _context.Set<PendingPasswordChange>()
                .FirstOrDefaultAsync(x => x.Email == email && x.ExpiryAt > DateTime.UtcNow);
            return record?.HashedNewPassword;
        }

        public async Task DeletePendingChangeAsync(string email)
        {
            var record = await _context.Set<PendingPasswordChange>()
                .FirstOrDefaultAsync(x => x.Email == email);

            if (record != null)
            {
                _context.Remove(record);
                await _context.SaveChangesAsync();
            }
        }
    }
}
