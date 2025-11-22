using CinemaGrpc;

namespace PaymentService.ServiceConnector.CinemaService
{
    public class CinemaServiceConnector : BaseServiceConnector
    {
        private readonly ServiceConnectorConfig _serviceConnectorConfig;

        public CinemaServiceConnector(IConfiguration configuration) : base(configuration)
        {
            _serviceConnectorConfig = GetServiceConnectorConfig();
        }
        public async Task<GetBookingGrpcReplyDTO> GetBooking(Guid bookingId)
        {
            using var channel = GetMovieServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetBookingGrpcRequestDTO
            {
                BookingId = bookingId.ToString()
            };

            return await client.GetBookingAsync(request);
        }

        public async Task<UpdateBookingGrpcReplyDTO> UpdateBooking(Guid bookingId, string status)
        {
            using var channel = GetMovieServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new UpdateBookingGrpcRequestDTO
            {
                BookingId = bookingId.ToString(),
                Status = status
            };

            return await client.UpdateBookingAsync(request);
        }
    }
}
