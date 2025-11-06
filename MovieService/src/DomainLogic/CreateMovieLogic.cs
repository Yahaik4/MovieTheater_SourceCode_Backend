using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
{
    public class CreateMovieLogic : IDomainLogic<CreateMovieParam, Task<CreateMovieResultData>>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IMovieGenreRepository _movieGenreRepository;
        private readonly IMoviePersonRepository _moviePersonRepository;

        public CreateMovieLogic(IGenreRepository genreRepository, 
                                IPersonRepository personRepository, 
                                IMovieRepository movieRepository, 
                                IMovieGenreRepository movieGenreRepository, 
                                IMoviePersonRepository moviePersonRepository)
        {
            _genreRepository = genreRepository;
            _personRepository = personRepository;
            _movieRepository = movieRepository;
            _moviePersonRepository = moviePersonRepository;
            _movieGenreRepository = movieGenreRepository;
        }

        public async Task<CreateMovieResultData> Execute(CreateMovieParam param)
        {
            var genreIds = param.Genres.Select(g => g.GenreId).ToList();
            var personIds = param.Persons.Select(p => p.PersonId).ToList();
            var distinctPersonIds = personIds.Distinct().ToList();

            var existingGenres = await _genreRepository.GetGenreByIds(genreIds);
            var existingPersons = await _personRepository.GetPersonByIds(personIds);

            if (existingGenres.Count() != genreIds.Count)
                throw new NotFoundException("Some genres do not exist.");

            if (existingPersons.Count() != distinctPersonIds.Count)
                throw new NotFoundException("Some persons do not exist.");

            var movieId = Guid.NewGuid();
            var movie = new Movie
            {
                Id = movieId,
                Name = param.Name,
                Description = param.Description,
                ReleaseDate = param.ReleaseDate,
                Duration = param.Duration,
                Publisher = param.Publisher,
                Country = param.Country,
                Language = param.Language,
                Poster = param.Poster,
                TrailerUrl = param.TrailerUrl,
                Status = param.Status,
            };

            var newMovie = await _movieRepository.CreateMovie(movie);

            var movieGenres = await _movieGenreRepository.CreateMovieGenre(movieId, param.Genres);
            var moviePersons = await _moviePersonRepository.CreateMoviePerson(movieId, param.Persons);

            return new CreateMovieResultData
            {
                Result = true,
                Message = "Create Movie Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new CreateMovieDataResult
                {
                    Id = newMovie.Id,
                    Name = newMovie.Name,
                    Description = newMovie.Description,
                    ReleaseDate = newMovie.ReleaseDate,
                    Duration = newMovie.Duration,
                    Publisher = newMovie.Publisher,
                    Country = newMovie.Country,
                    Language = newMovie.Language,
                    Poster = newMovie.Poster,
                    TrailerUrl = newMovie.TrailerUrl,
                    Status = newMovie.Status,
                    Genres = movieGenres.Select(mg => new MovieGenreDataResult
                    {
                        GenreId = mg.GenreId,
                        GenreName = mg.Genre.Name,
                    }).ToList(),
                    Persons = moviePersons.Select(mp => new MoviePersonDataResult
                    {
                        PersonId = mp.Id,
                        FullName = mp.Person.FullName,
                        Role = mp.Role
                    }).ToList()
                }
            };
        }
    }
}
