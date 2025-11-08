namespace ApiGateway.DataTransferObject.ResultData
{
    public class GetAllCinemasResultDTO : BaseResultDTO
    {
        public List<GetAllCinemasDataResult> Data { get; set; }
    }

    public class GetAllCinemasDataResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Open_Time { get; set; }
        public string Close_Time { get; set; }
        public int TotalRoom { get; set; }
        public string Status { get; set; }
    }
}
