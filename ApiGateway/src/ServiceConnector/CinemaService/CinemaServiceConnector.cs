using ApiGateway.DataTransferObject.Parameter;
using ApiGateway.Helper;
using CinemaGrpc;

namespace ApiGateway.ServiceConnector.CinemaService
{
    public class CinemaServiceConnector : BaseServiceConnector
    {
        private readonly ServiceConnectorConfig _serviceConnectorConfig;
        private readonly ICurrentUserService _currentUserService;

        public CinemaServiceConnector(IConfiguration configuration, ICurrentUserService currentUserService) : base(configuration)
        {
            _serviceConnectorConfig = GetServiceConnectorConfig();
            _currentUserService = currentUserService;
        }

        public async Task<GetAllCinemasGrpcReplyDTO> GetAllCinemas(Guid? id, string? name,string? city, string? status)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetAllCinemasGrpcRequestDTO();

            if (id.HasValue)
                request.Id = id.Value.ToString();

            if (!string.IsNullOrWhiteSpace(name))
                request.Name = name;

            if (!string.IsNullOrWhiteSpace(city))
                request.City = city;

            if (!string.IsNullOrWhiteSpace(status))
                request.Status = status;

            return await client.GetAllCinemasAsync(request);
        }

        public async Task<CreateCinemaGrpcReplyDTO> CreateCinema(string name, string address, string city, string phoneNumber, string email, string open_time, string close_time, string status)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new CreateCinemaGrpcRequestDTO
            {
                Name = name,
                Address = address,
                City = city,
                PhoneNumber = phoneNumber,
                Email = email,
                OpenTime = open_time,
                CloseTime = close_time,
                Status = status,
                CreatedBy = _currentUserService.UserId ?? _currentUserService.Email ?? "System"
            };

            return await client.CreateCinemaAsync(request);
        }

        public async Task<UpdateCinemaGrpcReplyDTO> UpdateCinema(Guid id, string name, string address, string city, string phoneNumber, string email, string open_time, string close_time, string status)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new UpdateCinemaGrpcRequestDTO
            {
                Id = id.ToString(),
                Name = name,
                Address = address,
                City = city,
                PhoneNumber = phoneNumber,
                Email = email,
                OpenTime = open_time,
                CloseTime = close_time,
                Status = status
            };

            return await client.UpdateCinemaAsync(request);
        }

        public async Task<DeleteCinemaGrpcReplyDTO> DeleteCinema(Guid id)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new DeleteCinemaGrpcRequestDTO
            {
                Id = id.ToString(),
            };

            return await client.DeleteCinemaAsync(request);
        }

        public async Task<GetAllRoomTypesGrpcReplyDTO> GetAllRoomTypes(Guid? id, string? type, decimal? basePrice)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetAllRoomTypesGrpcRequestDTO();

            if (id.HasValue)
                request.Id = id.Value.ToString();

            if (!string.IsNullOrWhiteSpace(type))
                request.Type = type;

            if (basePrice.HasValue)
                request.BasePrice = basePrice.Value.ToString();

            return await client.GetAllRoomTypesAsync(request);
        }

        public async Task<CreateRoomTypeGrpcReplyDTO> CreateRoomType(string type, decimal basePrice)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new CreateRoomTypeGrpcRequestDTO
            {
                Type = type,
                BasePrice = basePrice.ToString(),
                CreatedBy = _currentUserService.UserId ?? _currentUserService.Email ?? "System"
            };

            return await client.CreateRoomTypeAsync(request);
        }

        public async Task<UpdateRoomTypeGrpcReplyDTO> UpdateRoomType(Guid id, string type, decimal basePrice)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new UpdateRoomTypeGrpcRequestDTO
            {
                Id = id.ToString(),
                Type = type,
                BasePrice = basePrice.ToString(),
            };

            return await client.UpdateRoomTypeAsync(request);
        }

        public async Task<DeleteRoomTypeGrpcReplyDTO> DeleteRoomType(Guid id)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new DeleteRoomTypeGrpcRequestDTO
            {
                Id = id.ToString(),
            };

            return await client.DeleteRoomTypeAsync(request);
        }

        public async Task<GetAllSeatTypesGrpcReplyDTO> GetAllSeatTypes(Guid? id, string? type, decimal? extraPrice)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetAllSeatTypesGrpcRequestDTO();

            if (id.HasValue)
                request.Id = id.Value.ToString();

            if (!string.IsNullOrWhiteSpace(type))
                request.Type = type;

            if (extraPrice.HasValue)
                request.ExtraPrice = extraPrice.Value.ToString();

            return await client.GetAllSeatTypesAsync(request);
        }

        public async Task<CreateSeatTypeGrpcReplyDTO> CreateSeatType(string type, decimal extraPrice)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new CreateSeatTypeGrpcRequestDTO
            {
                Type = type,
                ExtraPrice = extraPrice.ToString(),
                CreatedBy = _currentUserService.UserId ?? _currentUserService.Email ?? "System"
            };

            return await client.CreateSeatTypeAsync(request);
        }

        public async Task<UpdateSeatTypeGrpcReplyDTO> UpdateSeatType(Guid id, string type, decimal extraPrice)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new UpdateSeatTypeGrpcRequestDTO
            {
                Id = id.ToString(),
                Type = type,
                ExtraPrice = extraPrice.ToString(),
            };

            return await client.UpdateSeatTypeAsync(request);
        }

        public async Task<DeleteSeatTypeGrpcReplyDTO> DeleteSeatType(Guid id)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new DeleteSeatTypeGrpcRequestDTO
            {
                Id = id.ToString(),
            };

            return await client.DeleteSeatTypeAsync(request);
        }

        public async Task<CreateRoomGrpcReplyDTO> CreateRoom(int roomNumber, int totalColumn, int totalRow, string status, Guid roomTypeId, Guid cinemaId)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new CreateRoomGrpcRequestDTO
            {
                RoomNumber = roomNumber,
                TotalColumn = totalColumn,
                TotalRow = totalRow,
                Status = status,
                RoomTypeId = roomTypeId.ToString(),
                CinemaId = cinemaId.ToString(),
                CreatedBy = _currentUserService.UserId ?? _currentUserService.Email ?? "System"
            };

            return await client.CreateRoomAsync(request);
        }

        public async Task<GetAllRoomsGrpcReplyDTO> GetAllRooms(Guid cinemaId, Guid? id, int? roomNumber, string? status, string? type)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetAllRoomsGrpcRequestDTO();

            request.CinemaId = cinemaId.ToString();

            if (id.HasValue)
                request.Id = id.Value.ToString();

            if (roomNumber.HasValue)
                request.RoomNumber = roomNumber.Value;

            if (!string.IsNullOrWhiteSpace(status))
                request.Status = status;

            if (!string.IsNullOrWhiteSpace(type))
                request.Type = type;

            return await client.GetAllRoomsAsync(request);
        }

        public async Task<UpdateRoomGrpcReplyDTO> UpdateRoom(Guid id, int roomNumber, string status, int totalColumn, int totalRow, Guid roomTypeId, Guid cinemaId)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new UpdateRoomGrpcRequestDTO
            {
                Id = id.ToString(),
                RoomNumber = roomNumber,
                Status = status,
                TotalColumn = totalColumn,
                TotalRow = totalRow,
                RoomTypeId = roomTypeId.ToString(),
                CinemaId = cinemaId.ToString()
            };

            return await client.UpdateRoomAsync(request);
        }

        public async Task<DeleteRoomGrpcReplyDTO> DeleteRoom(Guid id)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new DeleteRoomGrpcRequestDTO
            {
                Id = id.ToString(),
            };

            return await client.DeleteRoomAsync(request);
        }

        public async Task<GetAllSeatsGrpcReplyDTO> GetAllSeats(Guid roomId)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetAllSeatsGrpcRequestDTO
            {
                RoomId = roomId.ToString(),
            };

            return await client.GetAllSeatsAsync(request);
        }

        public async Task<UpdateSeatsGrpcReplyDTO> UpdateSeats(List<Guid> ids, bool? isActive, Guid? seatTypeId)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new UpdateSeatsGrpcRequestDTO();

            request.Id.AddRange(ids.Select(id => id.ToString()));

            if (isActive.HasValue)
                request.IsActive = isActive.Value;

            if (seatTypeId.HasValue)
                request.SeatTypeId = seatTypeId.Value.ToString();

            return await client.UpdateSeatsAsync(request);
        }

        public async Task<GetShowtimesByRoomGrpcReplyDTO> GetShowtimesByRoom(Guid roomId, DateOnly from, DateOnly to)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetShowtimesByRoomGrpcRequestDTO
            {
                Id = roomId.ToString(),
                From = from.ToString(),
                To = to.ToString(),
            };

            return await client.GetShowtimesByRoomAsync(request);
        }

        public async Task<GetShowtimesGrpcReplyDTO> GetShowtimes(Guid? id, Guid movieId, DateOnly date, string? country)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetShowtimesGrpcRequestDTO
            {
                Id = id.ToString(),
                MovieId = movieId.ToString(),
                Date = date.ToString(),
                Country = country
            };

            return await client.GetShowtimesAsync(request);
        }

        public async Task<GetShowtimesByCinemaGrpcReplyDTO> GetShowtimesByCinema(Guid cinemaId, DateOnly date)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetShowtimesByCinemaGrpcRequestDTO
            {
                CinemaId = cinemaId.ToString(),
                Date = date.ToString()
            };

            return await client.GetShowtimesByCinemaAsync(request);
        }

        public async Task<GetShowtimeDetailsGrpcReplyDTO> GetShowtimeDetails(Guid showtimeId)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetShowtimeDetailsGrpcRequestDTO
            {
                ShowtimeId = showtimeId.ToString()
            };

            return await client.GetShowtimeDetailsAsync(request);
        }

        public async Task<CreateShowtimeGrpcReplyDTO> CreateShowtime(Guid movieId, Guid roomId, DateTime startTime, DateTime endTime, string status)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new CreateShowtimeGrpcRequestDTO
            {
                MovieId = movieId.ToString(),
                RoomId = roomId.ToString(),
                StartTime = startTime.ToString(),
                EndTime = endTime.ToString(),
                Status = status
            };

            return await client.CreateShowtimeAsync(request);
        }

        public async Task<UpdateShowtimeGrpcReplyDTO> UpdateShowtime(Guid id, UpdateShowtimeRequestParam param)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new UpdateShowtimeGrpcRequestDTO
            {
                Id = id.ToString(),
                MovieId = param.MovieId.ToString(),
                RoomId = param.RoomId.ToString(),
                StartTime = param.StartTime.ToString(),
                EndTime = param.EndTime.ToString(),
                Status = param.Status
            };

            return await client.UpdateShowtimeAsync(request);
        }

        public async Task<GetShowtimeSeatsGrpcReplyDTO> GetShowtimeSeats(Guid showtimeId)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetShowtimeSeatsGrpcRequestDTO
            {
                Id = showtimeId.ToString(),
            };

            return await client.GetShowtimeSeatsAsync(request);
        }

        public async Task<CreateBookingGrpcReplyDTO> CreateBooking(
            string userId, 
            Guid showtimeId, 
            Guid? promotionId,
            List<Guid> showtimeSeatIds,
            List<CreateBookingFoodDrinkRequestItem>? foodDrinkItems)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new CreateBookingGrpcRequestDTO
            {
                UserId = userId,
                ShowtimeId = showtimeId.ToString(),
                PromotionId = promotionId.ToString(),
            };

            request.ShowtimeSeatIds.AddRange(showtimeSeatIds.Select(x => x.ToString()));

            if (foodDrinkItems != null && foodDrinkItems.Any())
            {
                request.FoodDrinkItems.AddRange(
                    foodDrinkItems.Select(x => new CreateBookingFoodDrinkItemGrpcDTO
                    {
                        FoodDrinkId = x.FoodDrinkId.ToString(),
                        Quantity = x.Quantity
                    }));
            }

            return await client.CreateBookingAsync(request);
        }

        public async Task<UpdateBookingStatusGrpcReplyDTO> UpdateBookingStatus(Guid id, string status)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new UpdateBookingStatusGrpcRequestDTO
            {
                BookingId = id.ToString(),
                Status = status.ToString(),
            };

            return await client.UpdateBookingStatusAsync(request);
        }

        public async Task<GetAllFoodDrinksGrpcReplyDTO> GetAllFoodDrinks(Guid? id, string? name, string? type, string? size)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetAllFoodDrinksGrpcRequestDTO();

            if (id.HasValue)
                request.Id = id.Value.ToString();

            if (!string.IsNullOrWhiteSpace(name))
                request.Name = name;

            if (!string.IsNullOrWhiteSpace(type))
                request.Type = type;

            if (!string.IsNullOrWhiteSpace(size))
                request.Size = size;

            return await client.GetAllFoodDrinksAsync(request);
        }

        public async Task<CreateFoodDrinkGrpcReplyDTO> CreateFoodDrink(CreateFoodDrinkRequestParam param)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new CreateFoodDrinkGrpcRequestDTO
            {
                Name = param.Name,
                Type = param.Type,
                Size = param.Size,
                Price = param.Price.ToString(),
                Image = param.Image ?? "",
                Description = param.Description ?? "",
                CreatedBy = _currentUserService.UserId ?? _currentUserService.Email ?? "System",
            };

            return await client.CreateFoodDrinkAsync(request);
        }

        public async Task<CheckInBookingGrpcReplyDTO> CheckInBooking(Guid bookingId)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var staffUserId = _currentUserService.UserId;

            if (string.IsNullOrWhiteSpace(staffUserId))
            {
                throw new Exception("UserId not found in token");
            }

            var request = new CheckInBookingGrpcRequestDTO
            {
                BookingId = bookingId.ToString(),
                StaffUserId = staffUserId
            };

            return await client.CheckInBookingAsync(request);
        }

        public async Task<UpdateFoodDrinkGrpcReplyDTO> UpdateFoodDrink(
            Guid id,
            UpdateFoodDrinkRequestParam param)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new UpdateFoodDrinkGrpcRequestDTO
            {
                Id = id.ToString(),
                Name = param.Name ?? "",
                Type = param.Type ?? "",
                Size = param.Size ?? "",
                Price = param.Price.HasValue ? param.Price.Value.ToString() : "",
                Image = param.Image ?? "",          // empty => clear
                Description = param.Description ?? ""
            };

            return await client.UpdateFoodDrinkAsync(request);
        }

        public async Task<DeleteFoodDrinkGrpcReplyDTO> DeleteFoodDrink(Guid id)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new DeleteFoodDrinkGrpcRequestDTO
            {
                Id = id.ToString()
            };

            return await client.DeleteFoodDrinkAsync(request);
        }

        public async Task<GetBookingHistoryGrpcReplyDTO> GetBookingHistory(string userId)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetBookingHistoryGrpcRequestDTO
            {
                UserId = userId
            };

            return await client.GetBookingHistoryAsync(request);
        }

        public async Task<GetCustomerTypesGrpcReplyDTO> GetCustomerTypes(Guid? id, string? name, string? roleCondition)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetCustomerTypesGrpcRequestDTO();

            if (id.HasValue)
                request.Id = id.Value.ToString();

            if (!string.IsNullOrWhiteSpace(name))
                request.Name = name;

            if (!string.IsNullOrWhiteSpace(roleCondition))
            {
                request.RoleCondition = roleCondition;
            }

            return await client.GetCustomerTypesAsync(request);
        }

        public async Task<CreateCustomerTypeGrpcReplyDTO> CreateCustomerType(string name, string roleCondition)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new CreateCustomerTypeGrpcRequestDTO
            {
                Name = name,
                RoleCondition = roleCondition
            };

            return await client.CreateCustomerTypeAsync(request);
        }

        public async Task<UpdateCustomerTypeGrpcReplyDTO> UpdateCustomerType(Guid id, string? name, string? roleCondition)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new UpdateCustomerTypeGrpcRequestDTO
            {
                Id = id.ToString(),
                Name = name,
                RoleCondition = roleCondition
            };

            return await client.UpdateCustomerTypeAsync(request);
        }

        public async Task<GetHolidaysGrpcReplyDTO> GetHolidays(string? name, DateOnly? startDate, DateOnly? endDate)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetHolidaysGrpcRequestDTO();

            if (!string.IsNullOrWhiteSpace(name))
                request.Name = name;

            if (startDate.HasValue)
                request.StartDate = startDate.Value.ToString("yyyy-MM-dd");

            if (endDate.HasValue)
                request.EndDate = endDate.Value.ToString("yyyy-MM-dd");

            return await client.GetHolidaysAsync(request);
        }

        public async Task<CreateHolidayGrpcReplyDTO> CreateHoliday(string name, int day, int month, decimal extraPrice)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new CreateHolidayGrpcRequestDTO
            {
                Name = name,
                Day = day,
                Month = month,
                ExtraPrice = extraPrice.ToString()
            };

            return await client.CreateHolidayAsync(request);
        }

        public async Task<UpdateHolidayGrpcReplyDTO> UpdateHoliday(Guid id, string? name, int? day, int? month, decimal? extraPrice)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new UpdateHolidayGrpcRequestDTO
            {
                Id = id.ToString(),
                Name = name,
                Day = day.ToString(),
                Month = month.ToString(),
                ExtraPrice = extraPrice.ToString()
            };

            return await client.UpdateHolidayAsync(request);
        }

        public async Task<GetPromotionsGrpcReplyDTO> GetPromotions(Guid? id, string? code, DateTime? startDate, DateTime? endDate, string? discountType, bool? isActive)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new GetPromotionsGrpcRequestDTO
            {
                Id = id.ToString(),
                Code = code,
                StartDate = startDate.ToString(),
                EndDate = endDate.ToString(),
                DiscountType = discountType,
                IsActive = isActive,
            };

            return await client.GetPromotionsAsync(request);
        }

        public async Task<CreatePromotionGrpcReplyDTO> CreatePromotion(string code, 
                                                                       string description, 
                                                                       DateTime startDate, 
                                                                       DateTime endDate, 
                                                                       string discountType, 
                                                                       decimal discountValue,
                                                                       int? limitPerUser,
                                                                       int? limitTotalUse,
                                                                       decimal? minOrderValue,
                                                                       bool isActive)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new CreatePromotionGrpcRequestDTO
            {
                Code = code,
                Description = description,
                StartDate = startDate.ToString(),
                EndDate = endDate.ToString(),
                DiscountValue = discountValue.ToString(),
                LimitPerUser = limitPerUser,
                LimitTotalUse = limitTotalUse,
                MinOrderValue = minOrderValue.ToString(),
                DiscountType = discountType,
                IsActive = isActive,
            };

            return await client.CreatePromotionAsync(request);
        }

        public async Task<UpdatePromotionGrpcReplyDTO> UpdatePromotion(Guid id,
                                                                       string? code,
                                                                       string? description,
                                                                       DateTime? startDate,
                                                                       DateTime? endDate,
                                                                       string? discountType,
                                                                       decimal? discountValue,
                                                                       int? limitPerUser,
                                                                       int? limitTotalUse,
                                                                       decimal? minOrderValue,
                                                                       int? usedCount,
                                                                       bool? isActive)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new UpdatePromotionGrpcRequestDTO
            {
                Id = id.ToString(),
                Code = code,
                Description = description,
                StartDate = startDate.ToString(),
                EndDate = endDate.ToString(),
                DiscountValue = discountValue.ToString(),
                LimitPerUser = limitPerUser,
                LimitTotalUse = limitTotalUse,
                MinOrderValue = minOrderValue.ToString(),
                DiscountType = discountType,
                UsedCount = usedCount,
                IsActive = isActive,
            };

            return await client.UpdatePromotionAsync(request);
        }

        public async Task<SearchPromotionGrpcReplyDTO> SearchPromotion(string code)
        {
            using var channel = GetCinemaServiceChannel();
            var client = new CinemaGrpcService.CinemaGrpcServiceClient(channel);

            var request = new SearchPromotionGrpcRequestDTO
            {
                Code = code,
            };

            return await client.SearchPromotionAsync(request);
        }
    }
}
