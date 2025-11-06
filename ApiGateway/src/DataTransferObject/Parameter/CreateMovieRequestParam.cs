namespace src.DataTransferObject.Parameter
{
    public class CreateMovieRequestParam
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public TimeSpan Duration { get; set; }
        public string Publisher { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string Poster { get; set; }
        public string TrailerUrl { get; set; }
        public string Status { get; set; }
        public List<MovieGenreParam> Genres { get; set; }
        public List<MoviePersonParam> Persons { get; set; }
    }
    public class MovieGenreParam
    {
        public Guid GenreId { get; set; }
    }

    public class MoviePersonParam
    {
        public Guid PersonId { get; set; }
        public string Role { get; set; } = null!;
    }
}
