using Shared.Infrastructure;

namespace ProfileService.Infrastructure.EF.Models
{
    public class Staff : BaseEntity
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime? DayOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid CinemaId { get; set; }
        public decimal Salary { get; set; } = 0; 
        public string Position { get; set; } = string.Empty;
    }
}
