using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class UpdateBookingLogic : IDomainLogic<UpdateBookingParam, Task<UpdateBookingResultData>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IShowtimeSeatRepository _showtimeSeatRepository;

        public UpdateBookingLogic(IBookingRepository bookingRepository, IShowtimeSeatRepository showtimeSeatRepository)
        {
            _bookingRepository = bookingRepository;
            _showtimeSeatRepository = showtimeSeatRepository;
        }

        public async Task<UpdateBookingResultData> Execute(UpdateBookingParam param)
        {
            var booking = await _bookingRepository.GetBookingById(param.BookingId);

            if (booking == null) {
                throw new NotFoundException("Booking Not found");
            }

            var showtimeSeats = await _showtimeSeatRepository.GetShowtimeSeatsByShowtimeId(booking.ShowtimeId);

            if(param.Status == "paid")
            {
                booking.Status = param.Status;

                foreach (var seat in showtimeSeats) { 
                    seat.Status = "booked";
                }
            }

            if(param.Status == "cancelled" || param.Status == "failed")
            {
                booking.Status = param.Status;

                foreach (var seat in showtimeSeats)
                {
                    seat.Status = "available";
                }
            }
            
            await _bookingRepository.UpdateBooking(booking);
            await _showtimeSeatRepository.UpdateSeatsAsync(showtimeSeats.ToList());

            return new UpdateBookingResultData
            {
                Result = true,
                Message = "Update Booking Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new UpdateBookingDataResult
                {
                    BookingId = booking.Id,
                    Status = booking.Status,
                    BookingSeats = showtimeSeats.Select(sts => new UpdateBookingSeatsDataResult
                    {
                        SeatId = sts.Id,
                        Status = sts.Status,
                        SeatCode = sts.Seat.SeatCode,
                        SeatType = sts.Seat.SeatType.Type,
                        Label = sts.Seat.Label,
                        Price = sts.Price
                    }).ToList()
                }
            };
        }
    }
}
