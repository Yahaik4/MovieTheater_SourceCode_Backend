using Shared.Infrastructure;

namespace src.Infrastructure.EF.Models
{
    public class Customer : BaseEntity
    {
        public string FullName { get; set; }
        public string? PhoneNumer { get; set; }
        public DateOnly? DayOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public int Points { get; set; } = 0;
        public Guid UserId { get; set; }
    }
}
