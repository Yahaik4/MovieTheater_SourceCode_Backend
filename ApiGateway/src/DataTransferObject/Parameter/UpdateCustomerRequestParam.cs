namespace ApiGateway.DataTransferObject.Parameter
{
    public class UpdateCustomerRequestParam
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DayOfBirth { get; set; }
        public string? Gender { get; set; }
        public int? Points { get; set; }
    }
}
