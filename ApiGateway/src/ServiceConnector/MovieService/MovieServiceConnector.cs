using CinemaGrpc;
using Grpc.Core;
using MovieGrpc;
using src.Helper;

namespace src.ServiceConnector.MovieService
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
    }
}
