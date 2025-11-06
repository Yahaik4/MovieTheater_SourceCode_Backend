using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
{
    public class GetMoviesLogic : IDomainLogic<GetMoviesParam, Task<GetMoviesResultData>>
    {
        private readonly IMovieRepository _movieRepository;

        public GetMoviesLogic(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<GetMoviesResultData> Execute(GetMoviesParam param)
        {
            var movies = await _movieRepository.GetMovies(param);

            return new GetMoviesResultData
            {
                Result = true,
                Message = "Get Movies Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = movies.Select(m => new GetMoviesDataResult
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Country = m.Country,
                    Duration = m.Duration,
                    Language = m.Language,
                    Publisher = m.Publisher,
                    ReleaseDate = m.ReleaseDate,
                    Status = m.Status,
                    Poster = m.Poster,
                    TrailerUrl = m.TrailerUrl,
                    Genres = m.MovieGenres.Select(mg => new MovieGenreDataResult
                    {
                        GenreId = mg.GenreId,
                        GenreName = mg.Genre.Name,
                    }).ToList(),
                    Persons = m.MoviePersons.Select(mp => new MoviePersonDataResult
                    {
                        PersonId = mp.PersonId,
                        FullName = mp.Person.FullName,
                        Role = mp.Role
                    }).ToList(),
                }).ToList()
            };
        }
    }
}
