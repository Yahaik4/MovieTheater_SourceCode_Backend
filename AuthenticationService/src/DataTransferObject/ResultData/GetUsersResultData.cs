using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace AuthenticationService.DataTransferObject.ResultData
{
    public class GetUserWithProfileItem
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime? DayOfBirth { get; set; }
        public string? Gender { get; set; }

        public Guid? CinemaId { get; set; }
        public string? Position { get; set; }
        public decimal? Salary { get; set; }
    }

    public class GetUsersResultData : IResultData
    {
        public bool Result { get; set; }
        public string Message { get; set; } = string.Empty;
        public StatusCodeEnum StatusCode { get; set; }
        public List<GetUserWithProfileItem> Data { get; set; } = new();
    }
}
