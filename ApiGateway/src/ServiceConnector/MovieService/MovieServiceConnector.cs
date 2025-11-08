using ApiGateway.DataTransferObject.Parameter;
using ApiGateway.Helper;
using ApiGateway.ServiceConnector;
using CinemaGrpc;
using Grpc.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using MovieGrpc;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Xml.Linq;

namespace ApiGateway.ServiceConnector.MovieService
{
    public class MovieServiceConnector : BaseServiceConnector
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ServiceConnectorConfig _serviceConnectorConfig;

        public MovieServiceConnector(IConfiguration configuration, ICurrentUserService currentUserService) : base(configuration)
        {
            _currentUserService = currentUserService;
            _serviceConnectorConfig = GetServiceConnectorConfig();
        }

        public async Task<GetGenresGrpcReplyDTO> GetGenres(Guid? id, string? name)
        {
            using var channel = GetMovieServiceChannel();
            var client = new MovieGrpcService.MovieGrpcServiceClient(channel);

            var request = new GetGenresGrpcRequestDTO();

            if (id.HasValue)
                request.Id = id.Value.ToString();

            if (!string.IsNullOrWhiteSpace(name))
                request.Name = name;

            return await client.GetGenresAsync(request);
        }

        public async Task<CreateGenreGrpcReplyDTO> CreateGenre(string name)
        {
            using var channel = GetMovieServiceChannel();
            var client = new MovieGrpcService.MovieGrpcServiceClient(channel);

            var request = new CreateGenreGrpcRequestDTO
            {
                Name = name,
                CreatedBy = _currentUserService.UserId ?? _currentUserService.Email ?? "System"
            };

            return await client.CreateGenreAsync(request);
        }

        public async Task<UpdateGenreGrpcReplyDTO> UpdateGenre(Guid id, string name)
        {
            using var channel = GetMovieServiceChannel();
            var client = new MovieGrpcService.MovieGrpcServiceClient(channel);

            var request = new UpdateGenreGrpcRequestDTO
            {
                Id = id.ToString(),
                Name = name,
            };

            return await client.UpdateGenreAsync(request);
        }

        public async Task<DeleteGenreGrpcReplyDTO> DeleteGenre(Guid id)
        {
            using var channel = GetMovieServiceChannel();
            var client = new MovieGrpcService.MovieGrpcServiceClient(channel);

            var request = new DeleteGenreGrpcRequestDTO
            {
                Id = id.ToString()
            };

            return await client.DeleteGenreAsync(request);
        }

        public async Task<GetPersonsGrpcReplyDTO> GetPersons(Guid? id, string? name)
        {
            using var channel = GetMovieServiceChannel();
            var client = new MovieGrpcService.MovieGrpcServiceClient(channel);

            var request = new GetPersonsGrpcRequestDTO();

            if (id.HasValue)
                request.Id = id.Value.ToString();

            if (!string.IsNullOrWhiteSpace(name))
                request.Name = name;

            return await client.GetPersonsAsync(request);
        }

        public async Task<CreatePersonGrpcReplyDTO> CreatePerson(string fullName, string gender, DateOnly? birthDate, string? nationality, string? bio, string? imageUrl)
        {
            using var channel = GetMovieServiceChannel();
            var client = new MovieGrpcService.MovieGrpcServiceClient(channel);

            var request = new CreatePersonGrpcRequestDTO
            {
                FullName = fullName,
                Gender = gender,
                BirthDate = birthDate.ToString(),
                Nationality = nationality,
                Bio = bio,
                ImageUrl = imageUrl,
                CreatedBy = _currentUserService.UserId ?? _currentUserService.Email ?? "System"
            };

            return await client.CreatePersonAsync(request);
        }

        public async Task<UpdatePersonGrpcReplyDTO> UpdatePerson(Guid id, string fullName, string gender, DateOnly? birthDate, string? nationality, string? bio, string? imageUrl)
        {
            using var channel = GetMovieServiceChannel();
            var client = new MovieGrpcService.MovieGrpcServiceClient(channel);

            var request = new UpdatePersonGrpcRequestDTO
            {
                Id = id.ToString(),
                FullName = fullName,
                Gender = gender,
                BirthDate = birthDate.ToString(),
                Nationality = nationality,
                Bio = bio,
                ImageUrl = imageUrl
            };

            return await client.UpdatePersonAsync(request);
        }

        public async Task<DeletePersonGrpcReplyDTO> DeletePerson(Guid id)
        {
            using var channel = GetMovieServiceChannel();
            var client = new MovieGrpcService.MovieGrpcServiceClient(channel);

            var request = new DeletePersonGrpcRequestDTO
            {
                Id = id.ToString()
            };

            return await client.DeletePersonAsync(request);
        }

        public async Task<GetMoviesGrpcReplyDTO> GetMovies(Guid? id, string? name, string? country, string? status)
        {
            using var channel = GetMovieServiceChannel();
            var client = new MovieGrpcService.MovieGrpcServiceClient(channel);

            var request = new GetMoviesGrpcRequestDTO
            {
                Id = id.HasValue ? id.ToString() : null,
                Name = name,
                Country = country,
                Status = status
            };

            return await client.GetMoviesAsync(request);
        }

        public async Task<CreateMovieGrpcReplyDTO> CreateMovie(CreateMovieRequestParam param)
        {
            using var channel = GetMovieServiceChannel();
            var client = new MovieGrpcService.MovieGrpcServiceClient(channel);

            var request = new CreateMovieGrpcRequestDTO
            {
                Name = param.Name,
                Country = param.Country,
                Status = param.Status,
                Description = param.Description,
                Duration = param.Duration.ToString(),
                ReleaseDate = param.ReleaseDate.ToString(),
                Language = param.Language,
                Publisher = param.Publisher,
                Poster = param.Poster,
                TrailerUrl = param.TrailerUrl,
            };

            request.Genres.AddRange(param.Genres.Select(g =>
                new MovieGenreGrpcRequestDTO { GenreId = g.GenreId.ToString() }));

            request.Persons.AddRange(param.Persons.Select(p =>
                new MoviePersonGrpcRequestDTO
                {
                    PersonId = p.PersonId.ToString(),
                    Role = p.Role
                }));

            return await client.CreateMovieAsync(request);
        }

        public async Task<UpdateMovieGrpcReplyDTO> UpdateMovie(UpdateMovieRequestParam param)
        {
            using var channel = GetMovieServiceChannel();
            var client = new MovieGrpcService.MovieGrpcServiceClient(channel);

            var request = new UpdateMovieGrpcRequestDTO
            {
                Id = param.Id.ToString(),
                Name = param.Name,
                Country = param.Country,
                Status = param.Status,
                Description = param.Description,
                Duration = param.Duration.ToString(),
                ReleaseDate = param.ReleaseDate.ToString(),
                Language = param.Language,
                Publisher = param.Publisher,
                Poster = param.Poster,
                TrailerUrl = param.TrailerUrl,
            };

            request.Genres.AddRange(param.Genres.Select(g =>
                new MovieGenreGrpcRequestDTO { GenreId = g.GenreId.ToString() }));

            request.Persons.AddRange(param.Persons.Select(p =>
                new MoviePersonGrpcRequestDTO
                {
                    PersonId = p.PersonId.ToString(),
                    Role = p.Role
                }));

            return await client.UpdateMovieAsync(request);
        }
    }
}
