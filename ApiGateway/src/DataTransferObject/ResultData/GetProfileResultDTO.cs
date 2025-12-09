namespace ApiGateway.DataTransferObject.ResultData
{
    public class GetProfileResultDTO : BaseResultDTO
    {
        public GetProfileDataResult Data { get; set; }
    }

    public class GetProfileDataResult
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumer { get; set; }
        public DateOnly? DayOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public int Points { get; set; }
    }
}
