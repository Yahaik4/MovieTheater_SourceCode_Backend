using Grpc.Core;
using MovieGrpc;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

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

        public async Task<GetMoviesGrpcReplyDTO> GetMovieByIds(List<Guid> ids)
        {
            using var channel = GetMovieServiceChannel();
            var client = new MovieGrpcService.MovieGrpcServiceClient(channel);

            var request = new GetMoviesByIdsGrpcRequestDTO();

            request.Id.AddRange(ids.Select(x => x.ToString()));

            return await client.GetMoviesByIdsAsync(request);
        }
    }
}
