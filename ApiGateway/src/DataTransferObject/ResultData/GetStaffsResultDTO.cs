namespace ApiGateway.DataTransferObject.ResultData
{
    public class GetStaffsResultDTO : BaseResultDTO
    {
        public List<StaffDTO> Data { get; set; } = new();
    }

    public class StaffDTO
    {

        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsVerified { get; set; }


        public string PhoneNumber { get; set; } = string.Empty;
        public string DayOfBirth { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        public Guid CinemaId { get; set; }
        public string Position { get; set; } = string.Empty;
        public decimal Salary { get; set; }
    }
}
