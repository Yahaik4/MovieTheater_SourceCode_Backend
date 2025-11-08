namespace ApiGateway.DataTransferObject.ResultData
{
    public class CreatePersonResultDTO : BaseResultDTO
    {
        public CreatePersonDataResult Data { get; set; }
    }

    public class CreatePersonDataResult 
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
