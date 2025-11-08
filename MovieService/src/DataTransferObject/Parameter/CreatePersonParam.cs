using Shared.Contracts.Interfaces;

namespace MovieService.DataTransferObject.Parameter
{
    public class CreatePersonParam : IParam
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Nationality { get; set; }
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
        public string CreatedBy { get; set; }
    }
}
