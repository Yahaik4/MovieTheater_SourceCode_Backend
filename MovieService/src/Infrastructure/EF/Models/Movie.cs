using Shared.Infrastructure;

namespace src.Infrastructure.EF.Models
{
    public class Movie : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Duration { get; set; }
        public string Publisher { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string Poster { get; set; }
        public string Status { get; set; }
        public ICollection<MoviePerson> MoviePersons { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
