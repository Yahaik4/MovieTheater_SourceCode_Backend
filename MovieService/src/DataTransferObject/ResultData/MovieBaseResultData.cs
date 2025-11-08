namespace MovieService.DataTransferObject.ResultData
{
    public class MovieBaseResultData
    {
        public Guid Id { get; set; }
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

        public List<MovieGenreDataResult> Genres { get; set; }
        public List<MoviePersonDataResult> Persons { get; set; }
    }

    public class MovieGenreDataResult
    {
        public Guid GenreId { get; set; }
        public string GenreName { get; set; }
    }

    public class MoviePersonDataResult
    {
        public Guid PersonId { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
