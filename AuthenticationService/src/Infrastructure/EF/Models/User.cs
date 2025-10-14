using Shared.Shared.Infrastructure;

namespace src.Infrastructure.EF.Models
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer";
        public ICollection<Session> Sessions { get; set; }
    }
}
