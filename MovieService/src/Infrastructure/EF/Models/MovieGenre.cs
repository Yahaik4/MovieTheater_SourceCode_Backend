using Shared.Infrastructure;

namespace src.Infrastructure.EF.Models
{
    public class MovieGenre : BaseEntity
    {
        public Guid MovieId { get; set; }
        public Guid GenreId { get; set; }
        public Movie Movie { get; set; }
        public Genre Genre { get; set; }
    }
}
