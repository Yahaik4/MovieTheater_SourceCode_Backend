namespace AuthenticationService.DataTransferObject.ResultData
{
    public class GetStaffResultData
    {
        public Guid UserId { get; set; }
        public Guid CinemaId { get; set; }
        public string Position { get; set; } = string.Empty;


        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DayOfBirth { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public decimal Salary { get; set; }
    }
}
