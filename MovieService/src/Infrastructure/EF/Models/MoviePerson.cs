using Shared.Infrastructure;

namespace src.Infrastructure.EF.Models
{
    public class MoviePerson : BaseEntity
    {
        public Guid MovieId { get; set; }
        public Guid PersonId { get; set; }
        public string Role { get; set; }
        public Movie Movie { get; set; }
        public Person Person { get; set; }
    }
}
