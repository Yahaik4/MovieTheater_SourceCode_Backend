using MovieService.DataTransferObject.Parameter;
using MovieService.DataTransferObject.ResultData;
using MovieService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using MovieService.Infrastructure.EF.Models;

namespace MovieService.DomainLogic
{
    public class UpdateMovieLogic : IDomainLogic<UpdateMovieParam, Task<UpdateMovieResultData>>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IMovieGenreRepository _movieGenreRepository;
        private readonly IMoviePersonRepository _moviePersonRepository;

        public UpdateMovieLogic(IGenreRepository genreRepository,
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

        public async Task<UpdateMovieResultData> Execute(UpdateMovieParam param)
        {
            // Validation param
            if (param == null)
            {
                throw new ValidationException("Param cannot be null");
            }

            var hasAnyField =
                param.Name != null ||
                param.Description != null ||
                param.ReleaseDate.HasValue ||
                param.Duration.HasValue ||
                param.Publisher != null ||
                param.Country != null ||
                param.Language != null ||
                param.Poster != null ||
                param.TrailerUrl != null ||
                param.Status != null ||
                param.Genres != null && param.Genres.Any() ||
                param.Persons != null && param.Persons.Any();

            if (!hasAnyField)
            {
                throw new ValidationException("At least one field besides Id must be provided.");
            }
            var movie = await _movieRepository.GetMovieById(param.Id);

            // check movie existed
            if (movie == null)
            {
                throw new NotFoundException("Movie not found");
            }

            // update field for movie 
            if (param.Name != null)
            {
                movie.Name = param.Name;
            }

            if (param.Description != null)
            {
                movie.Description = param.Description;
            }

            if (param.ReleaseDate.HasValue)
            {
                movie.ReleaseDate = param.ReleaseDate.Value;
            }

            if (param.Duration.HasValue)
            {
                movie.Duration = param.Duration.Value;
            }

            if (param.Publisher != null)
            {
                movie.Publisher = param.Publisher;
            }
            if (param.Country != null)
            {
                movie.Country = param.Country;
            }
            if (param.Language != null)
            {
                movie.Language = param.Language;
            }

            if (param.Poster != null)
            {
                movie.Poster = param.Poster;
            }

            if (param.TrailerUrl != null)
            {
                movie.TrailerUrl = param.TrailerUrl;
            }

            if (param.Status != null)
            {
                movie.Status = param.Status;
            }

            // update movieGenres
            if (param.Genres != null)
            {
                var genreIds = param.Genres.Select(g => g.GenreId).ToList(); // 1, 2, 3   
                var existingGenres = await _genreRepository.GetGenreByIds(genreIds);

                if (existingGenres.Count() != genreIds.Count)
                {
                    throw new NotFoundException("Some genres do not exist.");
                }

                var currentGenresIds = movie.MovieGenres.Select(g => g.GenreId).ToList(); // 2, 3, 4
                var genreToAddIds = genreIds.Except(currentGenresIds).ToList(); // 1 (phan tu trong A khong co trong B)
                var genreToRemoveIds = currentGenresIds.Except(genreIds).ToList(); // 4 (phan tu trong B khogn co trong A)

                // lay ra danh sach genre can xoa
                var toRemove = movie.MovieGenres
                    .Where(mg => genreToRemoveIds.Contains(mg.GenreId))
                    .ToList();

                // xoa danh sach
                foreach (var mg in toRemove)
                {
                    movie.MovieGenres.Remove(mg);
                }

                // them moi
                foreach (var genreId in genreToAddIds)
                {
                    movie.MovieGenres.Add(new MovieGenre
                    {
                        MovieId = movie.Id,
                        GenreId = genreId
                    });
                }
            }

            if (param.Persons != null)
            {
                var personIds = param.Persons.Select(p => p.PersonId).ToList(); // 1, 1, 2, 3 ...
                var distinctPersonIds = personIds.Distinct().ToList(); // 1, 2, 3

                var existingPersons = await _personRepository.GetPersonByIds(personIds);

                if (existingPersons.Count() != distinctPersonIds.Count)
                    throw new NotFoundException("Some persons do not exist.");

                var currentPersonsIds = movie.MoviePersons.Select(mp => mp.PersonId).ToList(); // 3, 4, 5, ..... 

                await _moviePersonRepository.DeleteMoviePerson(movie.Id, currentPersonsIds); // remove all person
                await _moviePersonRepository.CreateMoviePerson(movie.Id, param.Persons); // create all person
            }


            movie.UpdatedAt = DateTime.UtcNow;
            await _movieRepository.UpdateMovie(movie);

            return new UpdateMovieResultData
            {
                Result = true,
                Message = "Update Movie Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new UpdateMovieDataResult
                {
                    Id = movie.Id,
                    Description = movie.Description,
                    Duration = movie.Duration,
                    ReleaseDate = movie.ReleaseDate,
                    Country = movie.Country,
                    Language = movie.Language,
                    Name = movie.Name,
                    Poster  = movie.Poster,
                    Publisher = movie.Publisher,
                    Status = movie.Status,
                    TrailerUrl = movie.TrailerUrl,
                    Genres = movie.MovieGenres.Select(mg => new MovieGenreDataResult
                    {
                        GenreId = mg.GenreId,
                        GenreName = mg.Genre.Name,
                    }).ToList(),
                    Persons = movie.MoviePersons.Select(mp => new MoviePersonDataResult
                    {
                        PersonId = mp.Id,
                        FullName = mp.Person.FullName,
                        Role = mp.Role,
                    }).ToList()
                }
            };
        }
    }
}
