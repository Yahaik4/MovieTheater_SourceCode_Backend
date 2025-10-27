namespace src.DataTransferObject.ResultData
{
    public class UpdateCinemaResultDTO : BaseResultDTO
    {
        public UpdateCinemaDataResult Data { get; set; }
    }

    public class UpdateCinemaDataResult
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public TimeOnly Open_Time { get; set; }
        public TimeOnly Close_Time { get; set; }
        public int TotalRoom { get; set; } = 0;
        public string Status { get; set; }
    }
}
