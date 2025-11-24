namespace ApiGateway.DataTransferObject.ResultData
{
    public class GetCustomersResultDTO : BaseResultDTO
    {
        public List<CustomerDTO> Data { get; set; } = new();
    }

    public class CustomerDTO
    {

        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsVerified { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;
        public string DayOfBirth { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int Points { get; set; }
    }
}
