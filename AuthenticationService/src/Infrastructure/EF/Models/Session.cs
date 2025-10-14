using Shared.Shared.Infrastructure;

namespace src.Infrastructure.EF.Models
{
    public class Session : BaseEntity
    {
        public Guid UserId { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime ExpiresAt { get; set; }
        public string? Device { get; set; }
        public string? IpAddress { get; set; }
        public User User { get; set; }
    }
}
