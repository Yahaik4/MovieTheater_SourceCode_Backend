using Shared.Infrastructure;

namespace MovieService.Infrastructure.EF.Models
{
    public class Person : BaseEntity
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Nationality { get; set; }
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<MoviePerson> MoviePersons { get; set; }
    }
}
