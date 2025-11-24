namespace ApiGateway.DataTransferObject.Parameter
{
    public class UpdateStaffRequestParam
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DayOfBirth { get; set; }
        public string? Gender { get; set; }

        public Guid? CinemaId { get; set; }
        public string? Position { get; set; }
        public decimal? Salary { get; set; }
    }
}
