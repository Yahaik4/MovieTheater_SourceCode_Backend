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
        private readonly IBookingRepository _bookingRepository;

        public GetBookingLogic(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<GetBookingResultData> Execute(GetBookingParam param)
        {
            var booking = await _bookingRepository.GetBookingById(param.BookingId);

            if (booking == null)
            {
                throw new NotFoundException("Booking Not Found");
            }

            var showtimeSeats = await _bookingRepository.GetBookingById(param.BookingId);

            return new GetBookingResultData
            {
                Result = true,
                Message = "Get Booking Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new GetBookingDataResult
                {
                    UserId = booking.UserId,
                    Status = booking.Status,
                    TotalPrice = booking.TotalPrice
                }
            };
        }
    }
}
