using ProfileService.Infrastructure.EF.Models;

namespace ProfileService.Infrastructure.Repositories.Interfaces
{
    public interface IStaffRepository
    {
        Task<Staff?> GetStaffByUserId(Guid userId);
        Task<Staff> CreateStaff(Staff staff);
        Task<Staff> UpdateStaff(Staff staff);
        Task SoftDeleteByUserId(Guid userId);
        Task<Staff?> GetStaffAsync(Guid? userId, Guid? cinemaId);
    }
}
