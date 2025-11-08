using System;
using Shared.Infrastructure;

namespace src.Infrastructure.EF.Models
{
    public class PendingPasswordChange : BaseEntity
    {
        public required string Email { get; set; }
        public required string HashedNewPassword { get; set; }
        public DateTime ExpiryAt { get; set; } = DateTime.UtcNow.AddMinutes(5);
    }
}
