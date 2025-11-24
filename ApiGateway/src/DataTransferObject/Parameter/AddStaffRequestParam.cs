namespace ApiGateway.DataTransferObject.Parameter
{
    public class AddStaffRequestParam
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
        public string DayOfBirth { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        
        public Guid CinemaId { get; set; }  
        public string Position { get; set; } = string.Empty; 
        public decimal Salary {get; set; } = 0;
    }
}
