using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using CinemaService.ServiceConnector;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class CheckInBookingLogic : IDomainLogic<CheckInBookingParam, Task<CheckInBookingResultData>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ProfileServiceConnector _profileConnector;

        public CheckInBookingLogic(
            IBookingRepository bookingRepository,
            ProfileServiceConnector profileConnector)
        {
            _bookingRepository = bookingRepository;
            _profileConnector = profileConnector;
        }

        public async Task<CheckInBookingResultData> Execute(CheckInBookingParam param)
        {
            var booking = await _bookingRepository.GetBookingWithDetailsAsync(param.BookingId);
            if (booking == null)
                throw new NotFoundException("Booking not found");

            var showtime = booking.Showtime ?? throw new ValidationException("Showtime not found");
            var room = showtime.Room ?? throw new ValidationException("Room not found");
            var cinema = room.Cinema ?? throw new ValidationException("Cinema not found");
            var bookingCinemaId = cinema.Id;

            if (booking.Status == "used")
                throw new ConflictException("Booking already checked in");

            if (booking.Status != "paid")
                throw new ValidationException("Only paid bookings can be checked in");

            // var now = DateTime.UtcNow;
            // if (now < showtime.StartTime.AddMinutes(-30) || now > showtime.EndTime)
            //     throw new ValidationException("This booking is not in a valid time window for check-in");

            var staff = await _profileConnector.GetStaffs(param.StaffUserId, bookingCinemaId);
            if (!staff.Found)
                throw new UnauthorizedException("You cannot check in booking of this cinema");

            booking.Status = "used";
            await _bookingRepository.UpdateBooking(booking);

            return new CheckInBookingResultData
            {
                Result = true,
                Message = "Check-in successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new CheckInBookingDataResult
                {
                    BookingId = booking.Id,
                    Status = booking.Status,
                    CinemaId = cinema.Id,
                    CinemaName = cinema.Name,
                    ShowtimeStartTime = showtime.StartTime,
                    ShowtimeEndTime = showtime.EndTime,
                    NumberOfSeats = booking.NumberOfSeats
                }
            };
        }
    }
}
