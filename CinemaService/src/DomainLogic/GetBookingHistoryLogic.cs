using ApiGateway.ServiceConnector.MovieService;
using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class GetBookingHistoryLogic : IDomainLogic<GetBookingHistoryParam, Task<GetBookingHistoryResultData>>
    {
        private readonly MovieServiceConnector _movieServiceConnector;
        private readonly IBookingRepository _bookingRepository;

        public GetBookingHistoryLogic(IBookingRepository bookingRepository, MovieServiceConnector movieServiceConnector)
        {
            _bookingRepository = bookingRepository;
            _movieServiceConnector = movieServiceConnector;
        }

        public async Task<GetBookingHistoryResultData> Execute(GetBookingHistoryParam param)
        {
            var bookings = await _bookingRepository.GetBookingByUserId(param.UserId);

            if (bookings == null || !bookings.Any())
            {
                return new GetBookingHistoryResultData
                {
                    Result = true,
                    Message = "No booking history found",
                    StatusCode = StatusCodeEnum.Success,
                    Data = new List<CreateBookingDataResult>()
                };
            }

            var resultList = new List<CreateBookingDataResult>();

            foreach (var booking in bookings)
            {
                var movie = await _movieServiceConnector.GetMovies(booking.Showtime.MovieId, null, null, null);

                var dto = new CreateBookingDataResult
                {
                    BookingId = booking.Id,
                    CinemaName = booking.Showtime.Room.Cinema.Name,
                    MovieName = movie != null ? movie.Data.First().Name : string.Empty, // fallback nếu null
                    RoomNumber = booking.Showtime.Room.RoomNumber,
                    StartTime = booking.Showtime.StartTime,
                    EndTime = booking.Showtime.EndTime,
                    NumberOfSeats = booking.NumberOfSeats,
                    TotalPrice = booking.TotalPrice,

                    BookingSeats = booking.BookingSeats.Select(s => new BookingSeatsDataResult
                    {
                        SeatId = s.SeatId,
                        SeatCode = s.SeatCode,
                        SeatType = s.SeatType,
                        Label = s.Label,
                        Price = s.Price
                    }).ToList(),

                    BookingFoodDrinks = booking.BookingItems.Select(item => new BookingFoodDrinkDataResult
                    {
                        FoodDrinkId = item.FoodDrink.Id,
                        Name = item.FoodDrink.Name,
                        Type = item.FoodDrink.Type,
                        Size = item.FoodDrink.Size,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.UnitPrice * item.Quantity
                    }).ToList()
                };

                resultList.Add(dto);
            }

            return new GetBookingHistoryResultData
            {
                Result = true,
                Message = "Get History Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = resultList
            };
        }
    }
}

