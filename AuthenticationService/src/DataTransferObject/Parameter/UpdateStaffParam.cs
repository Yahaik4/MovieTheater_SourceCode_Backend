using Shared.Contracts.Interfaces;

namespace AuthenticationService.DataTransferObject.Parameter
{
    public class UpdateStaffParam : IParam
    {
        public Guid TargetUserId { get; set; }

        public string CallerRole { get; set; } = string.Empty;
        public string? CallerPosition { get; set; }

        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DayOfBirth { get; set; }
        public string? Gender { get; set; }

        public Guid? CinemaId { get; set; }
        public string? Position { get; set; }
        public decimal? Salary { get; set; }
    }
}