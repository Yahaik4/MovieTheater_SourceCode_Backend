using Shared.Infrastructure;

namespace MovieService.Infrastructure.EF.Models
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
