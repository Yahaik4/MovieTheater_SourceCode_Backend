using ApiGateway.ServiceConnector.MovieService;
using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using System.Collections.Generic;

namespace CinemaService.DomainLogic
{
    public class CreateBookingLogic : IDomainLogic<CreateBookingParam, Task<CreateBookingResultData>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IShowtimeSeatRepository _showtimeSeatRepository;
        private readonly IShowtimeRepository _showtimeRepository;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly MovieServiceConnector _movieServiceConnector;
        private readonly IFoodDrinkRepository _foodDrinkRepository;
        private readonly IPromotionRepository _promotionRepository;

        public CreateBookingLogic(IShowtimeSeatRepository showtimeSeatRepository, 
                                  IBookingRepository bookingRepository, 
                                  IShowtimeRepository showtimeRepository, 
                                  ICinemaRepository cinemaRepository,
                                  MovieServiceConnector movieServiceConnector,
                                  IFoodDrinkRepository foodDrinkRepository,
                                  IPromotionRepository promotionRepository)
        {
            _showtimeSeatRepository = showtimeSeatRepository;
            _bookingRepository = bookingRepository;
            _showtimeRepository = showtimeRepository;
            _cinemaRepository = cinemaRepository;
            _movieServiceConnector = movieServiceConnector;
            _foodDrinkRepository = foodDrinkRepository; 
            _promotionRepository = promotionRepository;
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

            var seatsTotalPrice = showtimeSeats.Sum(s => s.Price);
            decimal foodTotalPrice = 0;

            var bookingItems = new List<BookingItem>();

            if (param.FoodDrinkItems != null && param.FoodDrinkItems.Any())
            {
                if (param.FoodDrinkItems.Any(x => x.Quantity <= 0))
                    throw new ValidationException("Food & drink quantity must be greater than 0");

                var groupedItems = param.FoodDrinkItems
                    .GroupBy(x => x.FoodDrinkId)
                    .Select(g => new
                    {
                        FoodDrinkId = g.Key,
                        Quantity = g.Sum(x => x.Quantity)
                    })
                    .ToList();

                var foodIds = groupedItems.Select(x => x.FoodDrinkId).Distinct().ToList();
                var foodEntities = await _foodDrinkRepository.GetByIdsAsync(foodIds);

                if (foodEntities.Count != foodIds.Count)
                    throw new ValidationException("Some foods/drinks are not available");

                foreach (var item in groupedItems)
                {
                    var food = foodEntities.First(x => x.Id == item.FoodDrinkId);

                    var unitPrice = food.Price;
                    var totalPriceItem = unitPrice * item.Quantity;

                    foodTotalPrice += totalPriceItem;

                    bookingItems.Add(new BookingItem
                    {
                        Id = Guid.NewGuid(),
                        BookingId = Guid.Empty,
                        ItemId = food.Id,
                        Quantity = item.Quantity,
                        UnitPrice = unitPrice,
                        TotalPrice = totalPriceItem
                    });
                }
            }

            var totalPrice = seatsTotalPrice + foodTotalPrice;

            if (param.PromotionId.HasValue)
            {
                var promotion = await _promotionRepository.GetPromotionById(param.PromotionId.Value);

                if (promotion == null)
                {
                    throw new NotFoundException("Promotion not found");
                }

                if (promotion.MinOrderValue != null)
                {
                    if(totalPrice < promotion.MinOrderValue)
                    {
                        throw new ValidationException($"Order total must be at least {promotion.MinOrderValue} to use this promotion.");
                    }
                }

                if (promotion.LimitTotalUse != null)
                {
                    if (promotion.UsedCount >= promotion.LimitTotalUse)
                    {
                        throw new ValidationException("This promotion has reached its maximum number of uses.");
                    }
                }

                if (promotion.LimitPerUser != null)
                {
                    var history = await _bookingRepository.GetBookingByUserId(param.UserId);
                    var totalUsedDiscount = history?.Count(b => b.PromotionId == param.PromotionId) ?? 0;

                    if (totalUsedDiscount >= promotion.LimitPerUser)
                    {
                        throw new ValidationException("This promotion has reached the maximum usage limit for this user.");
                    }
                }

                switch (promotion.DiscountType.ToLower())
                {
                    case "percentage":
                        totalPrice -= totalPrice * (promotion.DiscountValue / 100m);
                        break;

                    case "amount":
                        totalPrice -= promotion.DiscountValue;
                        break;

                    default:
                        throw new ValidationException("Invalid promotion discount type.");
                }

                totalPrice = Math.Max(0, totalPrice);

                promotion.UsedCount += 1;

                await _promotionRepository.UpdatePromotion(promotion);
            }

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                ShowtimeId = param.ShowtimeId,
                PromotionId = param.PromotionId,
                UserId = param.UserId,
                Status = "pending",
                ExpiredAt = DateTime.UtcNow + TimeSpan.FromMinutes(30),
                NumberOfSeats = showtimeSeats.Count,
                TotalPrice = totalPrice,
                ShowtimeSeats = showtimeSeats,
                BookingSeats = showtimeSeats.Select(sts => new BookingSeat
                {
                    SeatId = sts.Seat.Id,
                    SeatCode = sts.Seat.SeatCode,
                    SeatType = sts.Seat.SeatType.Type,
                    Label = sts.Seat.Label,
                    Price = sts.Price
                }).ToList(),
                BookingItems = bookingItems
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
                    PromotionId = newBooking.PromotionId,
                    BookingSeats = newBooking.BookingSeats.Select(bs => new BookingSeatsDataResult
                    {
                        SeatId = bs.SeatId,
                        SeatCode = bs.SeatCode,
                        SeatType = bs.SeatType,
                        Label = bs.Label,
                        Price = bs.Price,
                    }).ToList(),

                    // NEW
                    BookingFoodDrinks = newBooking.BookingItems?
                        .Select(bi => new BookingFoodDrinkDataResult
                        {
                            FoodDrinkId = bi.ItemId,
                            Name       = bi.FoodDrink?.Name,
                            Type       = bi.FoodDrink?.Type,
                            Size       = bi.FoodDrink?.Size,
                            Quantity   = bi.Quantity,
                            UnitPrice  = bi.UnitPrice,
                            TotalPrice = bi.TotalPrice
                        })
                        .ToList() ?? new List<BookingFoodDrinkDataResult>()
                }
            };
        }
    }
}
