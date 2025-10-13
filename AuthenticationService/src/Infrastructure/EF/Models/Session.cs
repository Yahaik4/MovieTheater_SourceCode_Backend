namespace src.Infrastructure.EF.Models
{
    public class Session
    {
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime ExpiresAt { get; set; }
        public string? Device { get; set; }
        public string? IpAddress { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public User User { get; set; }
    }
}
