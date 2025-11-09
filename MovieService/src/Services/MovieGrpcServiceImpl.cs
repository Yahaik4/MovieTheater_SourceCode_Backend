using AutoMapper;
using Grpc.Core;
using MovieGrpc;
using MovieService.DataTransferObject.Parameter;
using MovieService.DomainLogic;
using System.Text.Json;

namespace MovieService.Services
{
    public class MovieGrpcServiceImpl : MovieGrpcService.MovieGrpcServiceBase
    {
        private readonly IMapper _mapper;
        private readonly GetGenresLogic _getGenresLogic;
        private readonly CreateGenreLogic _createGenreLogic;
        private readonly UpdateGenreLogic _updateGenreLogic;
        private readonly DeleteGenreLogic _deleteGenreLogic;
        private readonly GetPersonsLogic _getPersonsLogic;
        private readonly CreatePersonLogic _createPersonLogic;
        private readonly UpdatePersonLogic _updatePersonLogic;
        private readonly DeletePersonLogic _deletePersonLogic;
        private readonly GetMoviesLogic _getMoviesLogic;
        private readonly CreateMovieLogic _createMovieLogic;
        private readonly UpdateMovieLogic _updateMovieLogic;
        private readonly DeleteMovieLogic _deleteMovieLogic;

        public MovieGrpcServiceImpl(IMapper mapper, 
                                    GetGenresLogic getGenresLogic,
                                    CreateGenreLogic createGenreLogic,
                                    UpdateGenreLogic updateGenreLogic,
                                    DeleteGenreLogic deleteGenreLogic,
                                    GetPersonsLogic getPersonsLogic,
                                    CreatePersonLogic createPersonLogic,
                                    UpdatePersonLogic updatePersonLogic,
                                    DeletePersonLogic deletePersonLogic,
                                    GetMoviesLogic getMoviesLogic,
                                    CreateMovieLogic createMovieLogic,
                                    UpdateMovieLogic updateMovieLogic,
                                    DeleteMovieLogic deleteMovieLogic)
        {
            _mapper = mapper;
            _getGenresLogic = getGenresLogic;
            _createGenreLogic = createGenreLogic;
            _updateGenreLogic = updateGenreLogic;
            _deleteGenreLogic = deleteGenreLogic;

            _getPersonsLogic = getPersonsLogic;
            _createPersonLogic = createPersonLogic;
            _updatePersonLogic = updatePersonLogic;
            _deletePersonLogic = deletePersonLogic;

            _getMoviesLogic = getMoviesLogic;
            _createMovieLogic = createMovieLogic;
            _updateMovieLogic = updateMovieLogic;
            _deleteMovieLogic = deleteMovieLogic;
        }

        public override async Task<GetGenresGrpcReplyDTO> GetGenres(GetGenresGrpcRequestDTO request, ServerCallContext context)
        {
            Guid? genreId = null;
            if (!string.IsNullOrWhiteSpace(request.Id)
                && Guid.TryParse(request.Id, out var parsedId))
            {
                genreId = parsedId;
            }

            var result = await _getGenresLogic.Execute(new GetGenresParam
            {
                Id = genreId,
                Name = request.Name,
            });

            return _mapper.Map<GetGenresGrpcReplyDTO>(result);
        }

        public override async Task<CreateGenreGrpcReplyDTO> CreateGenre(CreateGenreGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _createGenreLogic.Execute(new CreateGenreParam
            {
                Name = request.Name,
            });

            return _mapper.Map<CreateGenreGrpcReplyDTO>(result);
        }

        public override async Task<UpdateGenreGrpcReplyDTO> UpdateGenre(UpdateGenreGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _updateGenreLogic.Execute(new UpdateGenreParam
            {
                Id = Guid.Parse(request.Id),
                Name = request.Name,
            });

            return _mapper.Map<UpdateGenreGrpcReplyDTO>(result);
        }

        public override async Task<DeleteGenreGrpcReplyDTO> DeleteGenre(DeleteGenreGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _deleteGenreLogic.Execute(new DeleteGenreParam
            {
                Id = Guid.Parse(request.Id)
            });

            return _mapper.Map<DeleteGenreGrpcReplyDTO>(result);
        }

        public override async Task<GetPersonsGrpcReplyDTO> GetPersons(GetPersonsGrpcRequestDTO request, ServerCallContext context)
        {
            Guid? personId = null;
            if (!string.IsNullOrWhiteSpace(request.Id)
                && Guid.TryParse(request.Id, out var parsedId))
            {
                personId = parsedId;
            }

            var result = await _getPersonsLogic.Execute(new GetPersonsParam
            {
                Id = personId,
                Name = request.Name,
            });

            return _mapper.Map<GetPersonsGrpcReplyDTO>(result);
        }

        public override async Task<CreatePersonGrpcReplyDTO> CreatePerson(CreatePersonGrpcRequestDTO request, ServerCallContext context)
        {
            DateOnly? birthDate = null;
            if (!string.IsNullOrWhiteSpace(request.BirthDate)
                && DateOnly.TryParse(request.BirthDate, out var parsedBirthDate))
            {
                birthDate = parsedBirthDate;
            }

            var result = await _createPersonLogic.Execute(new CreatePersonParam
            {
                FullName = request.FullName,
                Gender = request.Gender,
                BirthDate = birthDate,
                Nationality = string.IsNullOrWhiteSpace(request.Nationality) ? null : request.Nationality,
                Bio = string.IsNullOrWhiteSpace(request.Bio) ? null : request.Bio,
                ImageUrl = string.IsNullOrWhiteSpace(request.ImageUrl) ? null : request.ImageUrl,
                CreatedBy = request.CreatedBy,
            });

            return _mapper.Map<CreatePersonGrpcReplyDTO>(result);
        }

        public override async Task<UpdatePersonGrpcReplyDTO> UpdatePerson(UpdatePersonGrpcRequestDTO request, ServerCallContext context)
        {
            DateOnly? birthDate = null;
            if (!string.IsNullOrWhiteSpace(request.BirthDate)
                && DateOnly.TryParse(request.BirthDate, out var parsedBirthDate))
            {
                birthDate = parsedBirthDate;
            }

            var result = await _updatePersonLogic.Execute(new UpdatePersonParam
            {
                Id = Guid.Parse(request.Id),
                FullName = request.FullName,
                Gender = request.Gender,
                BirthDate = birthDate,
                Nationality = request.Nationality,
                Bio = request.Bio,
                ImageUrl = request.ImageUrl,
            });

            return _mapper.Map<UpdatePersonGrpcReplyDTO>(result);
        }

        public override async Task<DeletePersonGrpcReplyDTO> DeletePerson(DeletePersonGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _deletePersonLogic.Execute(new DeletePersonParam
            {
                Id = Guid.Parse(request.Id)
            });

            return _mapper.Map<DeletePersonGrpcReplyDTO>(result);
        }

        public override async Task<GetMoviesGrpcReplyDTO> GetMovies(GetMoviesGrpcRequestDTO request, ServerCallContext context)
        {
            Guid? movieId = null;
            if (!string.IsNullOrWhiteSpace(request.Id)
                && Guid.TryParse(request.Id, out var parsedId))
            {
                movieId = parsedId;
            }

            var result = await _getMoviesLogic.Execute(new GetMoviesParam
            {
                Id = movieId,
                Name = request.Name,
                Country = request.Country,
                Status = request.Status,
            });

            return _mapper.Map<GetMoviesGrpcReplyDTO>(result);
        }

        public override async Task<CreateMovieGrpcReplyDTO> CreateMovie(CreateMovieGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _createMovieLogic.Execute(new CreateMovieParam
            {
                Name = request.Name,
                Description = request.Description,
                Duration = TimeSpan.Parse(request.Duration),
                Language = request.Language,
                Publisher = request.Publisher,
                ReleaseDate = DateOnly.Parse(request.ReleaseDate),
                Poster = request.Poster,
                TrailerUrl = request.TrailerUrl,
                Country = request.Country,
                Status = request.Status,
                Genres = request.Genres.Select(mg => new MovieGenreParam
                {
                    GenreId = Guid.Parse(mg.GenreId),
                }).ToList(),
                Persons = request.Persons.Select(mp => new MoviePersonParam
                {
                    PersonId = Guid.Parse(mp.PersonId),
                    Role = mp.Role,
                }).ToList()
            });

            return _mapper.Map<CreateMovieGrpcReplyDTO>(result);
        }

        public override async Task<UpdateMovieGrpcReplyDTO> UpdateMovie(UpdateMovieGrpcRequestDTO request, ServerCallContext context)
        {
            Console.WriteLine("Huy");
            Console.WriteLine("=== Received UpdateMovie Request ===");
            Console.WriteLine(JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                WriteIndented = true // in ??p, d? ??c
            }));

            TimeSpan? duration = null;
            if (!string.IsNullOrWhiteSpace(request.Duration)
                && TimeSpan.TryParse(request.Duration, out var parsedDuration))
            {
                duration = parsedDuration;
            }

            DateOnly? releaseDate = null;
            if (!string.IsNullOrWhiteSpace(request.ReleaseDate)
                && DateOnly.TryParse(request.ReleaseDate, out var parsedReleaseDate))
            {
                releaseDate = parsedReleaseDate;
            }

            var result = await _updateMovieLogic.Execute(new UpdateMovieParam
            {
                Id = Guid.Parse(request.Id),
                Name = request.Name,
                Description = request.Description,
                Duration = duration,
                Language = request.Language,
                Publisher = request.Publisher,
                ReleaseDate = releaseDate,
                Poster = request.Poster,
                TrailerUrl = request.TrailerUrl,
                Country = request.Country,
                Status = request.Status,
                Genres = request.Genres.Select(mg => new MovieGenreParam
                {
                    GenreId = Guid.Parse(mg.GenreId),
                }).ToList(),
                Persons = request.Persons.Select(mp => new MoviePersonParam
                {
                    PersonId = Guid.Parse(mp.PersonId),
                    Role = mp.Role,
                }).ToList()
            });

            return _mapper.Map<UpdateMovieGrpcReplyDTO>(result);
        }

        public override async Task<DeleteMovieGrpcReplyDTO> DeleteMovie(DeleteMovieGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _deleteMovieLogic.Execute(new DeleteMovieParam
            {
                Id = Guid.Parse(request.Id)
            });

            return _mapper.Map<DeleteMovieGrpcReplyDTO>(result);
        }
    }
}
