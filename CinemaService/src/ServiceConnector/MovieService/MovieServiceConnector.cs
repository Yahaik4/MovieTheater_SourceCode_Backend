using MovieGrpc;

namespace ApiGateway.ServiceConnector.MovieService
{
    public class MovieServiceConnector : BaseServiceConnector
    {
        private readonly ServiceConnectorConfig _serviceConnectorConfig;

        public MovieServiceConnector(IConfiguration configuration) : base(configuration)
        {
            _serviceConnectorConfig = GetServiceConnectorConfig();
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
    }
}
