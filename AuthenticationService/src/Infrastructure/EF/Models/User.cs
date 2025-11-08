using Shared.Infrastructure;

namespace AuthenticationService.Infrastructure.EF.Models
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "customer";
        public ICollection<Session> Sessions { get; set; }
    }
}
