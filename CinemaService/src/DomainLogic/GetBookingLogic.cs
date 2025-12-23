using ApiGateway.ServiceConnector.MovieService;
using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using System.Text.Json;

namespace CinemaService.DomainLogic
{
    public class GetBookingLogic : IDomainLogic<GetBookingParam, Task<GetBookingResultData>>
    {
        private readonly MovieServiceConnector _movieServiceConnector;
        private readonly IBookingRepository _bookingRepository;
        private readonly IShowtimeRepository _showtimeRepository;
        private readonly IRoomRepository _roomRepository;

        public GetBookingLogic(IBookingRepository bookingRepository, MovieServiceConnector movieServiceConnector, IShowtimeRepository showtimeRepository, IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _movieServiceConnector = movieServiceConnector;
            _showtimeRepository = showtimeRepository;
            _roomRepository = roomRepository;
        }

        public async Task<GetBookingResultData> Execute(GetBookingParam param)
        {
            var booking = await _bookingRepository.GetBookingById(param.BookingId);

            if (booking == null)
            {
                throw new NotFoundException("Booking Not Found");
            }

            var movie = await _movieServiceConnector.GetMovies(booking.Showtime.MovieId, null, null, null);

            if(movie == null || !movie.Result || movie.Data.Count() <= 0)
            {
                throw new NotFoundException("Movie Not Found");
            }

            var showtime = await _showtimeRepository.GetShowtimeById(booking.ShowtimeId);
            var room = await _roomRepository.GetRoomById(showtime.RoomId);

            var json = JsonSerializer.Serialize(booking.BookingSeats);

            var seats = JsonSerializer.Deserialize<List<BookingSeatJson>>(json)
            .Select(s => new GetSeatsBooking
            {
                Id = s.SeatId,
                Label = s.Label,
                SeatType = s.SeatType
            })
            .ToList();

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
                    CinemaId = room.CinemaId,
                    Cinema = room.Cinema.Name,
                    Address = room.Cinema.Address,
                    RoomNumber = room.RoomNumber,
                    RoomType = room.RoomType.Type,
                    TotalPrice = booking.TotalPrice,
                    Seats = seats
                }
            };
        }
    }
}
