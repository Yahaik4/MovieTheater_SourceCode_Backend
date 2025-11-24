using Shared.Contracts.Interfaces;

namespace ProfileService.DataTransferObject.Parameter
{
    public class CreateProfileParam : IParam
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        // Common
        public string? PhoneNumber { get; set; }
        public DateTime? DayOfBirth { get; set; }
        public string? Gender { get; set; }

        // Staff only
        public decimal? Salary {get; set; }
        public Guid? CinemaId { get; set; }
        public string? Position { get; set; }
    }
}
