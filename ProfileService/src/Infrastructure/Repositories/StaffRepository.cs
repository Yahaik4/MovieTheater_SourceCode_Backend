using Microsoft.EntityFrameworkCore;
using ProfileService.Data;
using ProfileService.Infrastructure.EF.Models;
using ProfileService.Infrastructure.Repositories.Interfaces;

namespace ProfileService.Infrastructure.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly ProfileDbContext _context;

        public StaffRepository(ProfileDbContext context)
        {
            _context = context;
        }

        public async Task<Staff?> GetStaffByUserId(Guid userId)
        {
            return await _context.Staffs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task SoftDeleteByUserId(Guid userId)
        {
            var staff = await _context.Staffs
                .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted);

            if (staff == null)
                return;

            staff.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<Staff> CreateStaff(Staff staff)
        {
            await _context.Staffs.AddAsync(staff);
            await _context.SaveChangesAsync();
            return staff;
        }

        public async Task<Staff> UpdateStaff(Staff staff)
        {
            _context.Staffs.Update(staff); 
            await _context.SaveChangesAsync();
            return staff;
        }
    }
}
