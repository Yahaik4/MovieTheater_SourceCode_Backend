using ApiGateway.ServiceConnector.MovieService;
using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Helper;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class CreateBookingLogic : IDomainLogic<CreateBookingParam, Task<CreateBookingResultData>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IShowtimeSeatRepository _showtimeSeatRepository;
        private readonly IShowtimeRepository _showtimeRepository;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly MovieServiceConnector _movieServiceConnector;

        public CreateBookingLogic(IShowtimeSeatRepository showtimeSeatRepository, 
                                  IBookingRepository bookingRepository, 
                                  IShowtimeRepository showtimeRepository, 
                                  ICinemaRepository cinemaRepository,
                                  MovieServiceConnector movieServiceConnector)
        {
            _showtimeSeatRepository = showtimeSeatRepository;
            _bookingRepository = bookingRepository;
            _showtimeRepository = showtimeRepository;
            _cinemaRepository = cinemaRepository;
            _movieServiceConnector = movieServiceConnector;
        }

        public async Task<CreateBookingResultData> Execute(CreateBookingParam param)
        {
            using var transaction = await _showtimeSeatRepository.BeginTransactionAsync();

            var showtime = await _showtimeRepository.GetShowtimeById(param.ShowtimeId);

            if (showtime == null) {
                throw new NotFoundException("Showtime not found");
            } 

            var showtimeSeats = await _showtimeSeatRepository.GetSeatsForBookingAsync(param.ShowtimeSeatIds, param.ShowtimeId);

            if (showtimeSeats.Count != param.ShowtimeSeatIds.Count)
                throw new ConflictException("Some seats are no longer available");

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                ShowtimeId = param.ShowtimeId,
                UserId = param.UserId,
                Status = "pending",
                ExpiredAt = DateTime.UtcNow + TimeSpan.FromMinutes(5),
                NumberOfSeats = showtimeSeats.Count,
                TotalPrice = showtimeSeats.Sum(s => s.Price),
                ShowtimeSeats = showtimeSeats,
                BookingSeats = showtimeSeats.Select(sts => new BookingSeat
                {
                    SeatId = sts.Seat.Id,
                    SeatCode = sts.Seat.SeatCode,
                    SeatType = sts.Seat.SeatType.Type,
                    Label = sts.Seat.Label,
                    Price = sts.Price
                }).ToList()
            };

            var newBooking = await _bookingRepository.CreateBooking(booking);

            foreach(var showtimeSeat in showtimeSeats)
            {
                showtimeSeat.Status = "pending";
                showtimeSeat.BookingId = newBooking.Id;
            }

            await _showtimeSeatRepository.UpdateSeatsAsync(showtimeSeats);

            await transaction.CommitAsync();

            var cinema = await _cinemaRepository.GetCinemaById(showtime.Room.CinemaId);
            var movie = await _movieServiceConnector.GetMovies(showtime.MovieId, null, null, null);

            return new CreateBookingResultData
            {
                Result = true,
                Message = "Create booking successfully",
                StatusCode = StatusCodeEnum.Created,
                Data = new CreateBookingDataResult
                {
                    BookingId = newBooking.Id,
                    CinemaName = cinema.Name,
                    MovieName = movie.Data.First().Name,
                    RoomNumber = showtime.Room.RoomNumber,
                    NumberOfSeats = newBooking.NumberOfSeats,
                    TotalPrice = newBooking.TotalPrice,
                    StartTime = showtime.StartTime,
                    EndTime = showtime.EndTime,
                    BookingSeats = newBooking.BookingSeats.Select(bs => new BookingSeatsDataResult
                    {
                        SeatId = bs.SeatId,
                        SeatCode = bs.SeatCode,
                        SeatType = bs.SeatType,
                        Label = bs.Label,
                        Price = bs.Price,
                    }).ToList(),
                }
            };
        }
    }
}
