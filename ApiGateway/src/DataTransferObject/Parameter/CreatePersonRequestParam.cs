namespace src.DataTransferObject.Parameter
{
    public class CreatePersonRequestParam
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Nationality { get; set; }
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
    }
}
