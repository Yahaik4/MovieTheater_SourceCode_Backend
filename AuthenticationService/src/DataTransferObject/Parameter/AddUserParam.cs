using Shared.Contracts.Interfaces;

namespace AuthenticationService.DataTransferObject.Parameter
{
    public class AddUserParam : IParam
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }
        public DateTime? DayOfBirth { get; set; }
        public string? Gender { get; set; }

        public Guid? CinemaId { get; set; }
        public string? Position { get; set; }
        public decimal? Salary {get; set; }
    }
}
