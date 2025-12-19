using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class UpdateBookingStatusLogic : IDomainLogic<UpdateBookingStatusParam, Task<UpdateBookingStatusResultData>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IShowtimeSeatRepository _showtimeSeatRepository;
        private readonly IBookingItemRepository _bookingItemRepository;

        public UpdateBookingStatusLogic(IBookingRepository bookingRepository, IShowtimeSeatRepository showtimeSeatRepository, IBookingItemRepository bookingItemRepository)
        {
            _bookingRepository = bookingRepository;
            _showtimeSeatRepository = showtimeSeatRepository;
            _bookingItemRepository = bookingItemRepository;
        }

        public async Task<UpdateBookingStatusResultData> Execute(UpdateBookingStatusParam param)
        {
            var booking = await _bookingRepository.GetBookingById(param.BookingId);

            if (booking == null) {
                throw new NotFoundException("Booking Not found");
            }

            var bookedSeatIds = booking.BookingSeats.Select(bs => bs.SeatId).ToList();

            var showtimeSeats = await _showtimeSeatRepository.GetShowtimeSeatsByShowtimeId(booking.ShowtimeId);

            var seatsToUpdate = showtimeSeats.Where(s => bookedSeatIds.Contains(s.SeatId)).ToList();

            if (param.Status.ToLower() == "paid")
            {
                booking.Status = "paid";

                foreach (var seat in seatsToUpdate)
                {
                    seat.Status = "booked";
                }
            }

            if (param.Status == "cancelled" || param.Status == "failed")
            {
                booking.Status = param.Status;

                foreach (var seat in seatsToUpdate)
                {
                    seat.Status = "available";
                    seat.BookingId = null;
                }

                var bookingItem = await _bookingItemRepository.GetBookingItemsByBookingId(booking.Id);

                if (bookingItem.Any()) 
                {
                    await _bookingItemRepository.DeleteBookingItemsByBooking(booking.Id);
                }
            }
            
            await _bookingRepository.UpdateBooking(booking);
            await _showtimeSeatRepository.UpdateSeatsAsync(seatsToUpdate);

            return new UpdateBookingStatusResultData
            {
                Result = true,
                Message = "Update Booking Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new UpdateBookingStatusDataResult
                {
                    BookingId = booking.Id,
                    Status = booking.Status,
                    BookingSeats = seatsToUpdate.Select(sts => new UpdateBookingSeatsDataResult
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
