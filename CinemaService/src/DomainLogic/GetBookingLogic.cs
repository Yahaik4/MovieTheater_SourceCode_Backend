using ApiGateway.ServiceConnector.MovieService;
using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class GetBookingLogic : IDomainLogic<GetBookingParam, Task<GetBookingResultData>>
    {
        private readonly MovieServiceConnector _movieServiceConnector;
        private readonly IBookingRepository _bookingRepository;

        public GetBookingLogic(IBookingRepository bookingRepository, MovieServiceConnector movieServiceConnector)
        {
            _bookingRepository = bookingRepository;
            _movieServiceConnector = movieServiceConnector;
        }

        public async Task<GetBookingResultData> Execute(GetBookingParam param)
        {
            var booking = await _bookingRepository.GetBookingById(param.BookingId);

            if (booking == null)
            {
                throw new NotFoundException("Booking Not Found");
            }

            var movie = await _movieServiceConnector.GetMovies(booking.Showtime.MovieId, null, null, null);

            //Console.WriteLine("Movie:", movie.Data.First().Name);
            if(movie == null || !movie.Result || movie.Data.Count() <= 0)
            {
                throw new NotFoundException("Movie Not Found");
            }

            var showtimeSeats = await _bookingRepository.GetBookingById(param.BookingId);

            return new GetBookingResultData
            {
                Result = true,
                Message = "Get Booking Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new GetBookingDataResult
                {
                    BookingId = booking.Id,
                    UserId = booking.UserId,
                    MovieId = booking.Showtime.MovieId,
                    MovieName = movie.Data.First().Name,
                    StartTime = booking.Showtime.StartTime,
                    EndTime = booking.Showtime.EndTime,
                    Status = booking.Status,
                    TotalPrice = booking.TotalPrice
                }
            };
        }
    }
}
