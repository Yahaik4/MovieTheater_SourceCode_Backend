using Shared.Infrastructure;

namespace src.Infrastructure.EF.Models
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
