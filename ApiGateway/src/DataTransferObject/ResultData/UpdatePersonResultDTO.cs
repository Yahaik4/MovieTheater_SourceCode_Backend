namespace src.DataTransferObject.ResultData
{
    public class UpdatePersonResultDTO : BaseResultDTO
    {
        public UpdatePersonDataResult Data { get; set; }
    }

    public class UpdatePersonDataResult
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Nationality { get; set; }
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
    }
}
