using Shared.Contracts.ResultData;

namespace AuthenticationService.DataTransferObject.ResultData
{
    public class GetCustomersResultData : BaseResultData
    {
        public List<CustomerWithProfileResultData> Data { get; set; } = new();
    }

    public class CustomerWithProfileResultData
    {
        // Từ bảng Users
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsVerified { get; set; }

        // Từ bảng Customer (ProfileService)
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DayOfBirth { get; set; } = string.Empty;   // yyyy-MM-dd
        public string Gender { get; set; } = string.Empty;
        public int Points { get; set; }
    }
}
