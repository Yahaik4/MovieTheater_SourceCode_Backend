using Shared.Contracts.ResultData;

namespace AuthenticationService.DataTransferObject.ResultData
{
    public class GetStaffsResultData : BaseResultData
    {
        public List<StaffWithProfileResultData> Data { get; set; } = new();
    }

    public class StaffWithProfileResultData
    {
        // Từ bảng Users
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsVerified { get; set; }

        // Từ bảng Staff (ProfileService)
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DayOfBirth { get; set; } = string.Empty;   // yyyy-MM-dd
        public string Gender { get; set; } = string.Empty;

        public Guid CinemaId { get; set; }
        public string Position { get; set; } = string.Empty;
        public decimal Salary { get; set; }
    }
}
