using CinemaGrpc;
using Grpc.Core;
using MovieGrpc;

namespace src.ServiceConnector.MovieService
{
    public class MovieServiceConnector : BaseServiceConnector
    {
        private readonly ServiceConnectorConfig _serviceConnectorConfig;

        public MovieServiceConnector(IConfiguration configuration) : base(configuration)
        {
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

    }
}
