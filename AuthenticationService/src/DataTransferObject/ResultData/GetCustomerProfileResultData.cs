using System;

namespace AuthenticationService.DataTransferObject.ResultData
{
    public class GetCustomerProfileResultData
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DayOfBirth { get; set; } = string.Empty; 
        public string Gender { get; set; } = string.Empty;
        public int Points { get; set; }
    }
}
