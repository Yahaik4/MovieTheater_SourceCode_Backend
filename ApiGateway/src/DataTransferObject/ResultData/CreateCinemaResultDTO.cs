namespace src.DataTransferObject.ResultData
{
    public class CreateCinemaResultDTO : BaseResultDTO
    {
        public CreateCinemaDataResult Data { get; set; }
    }

    public class CreateCinemaDataResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public TimeOnly Open_Time { get; set; }
        public TimeOnly Close_Time { get; set; }
        public int TotalRoom { get; set; } = 0;
        public string Status { get; set; }
        public string CreateBy { get; set; }
    }
}
