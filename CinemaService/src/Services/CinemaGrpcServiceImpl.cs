using AutoMapper;
using CinemaGrpc;
using CinemaService.DataTransferObject.Parameter;
using CinemaService.DomainLogic;
using Grpc.Core;
using System.Globalization;
using System.Text.Json;
using CinemaService.DataTransferObject.ResultData;

namespace CinemaService.Services
{
    public class CinemaGrpcServiceImpl : CinemaGrpcService.CinemaGrpcServiceBase
    {
        private readonly IMapper _mapper;
        private readonly CreateCinemaLogic _createCinemaLogic;
        private readonly GetAllCinemaLogic _getAllCinemaLogic;
        private readonly UpdateCinemaLogic _updateCinemaLogic;
        private readonly DeleteCinemaLogic _deleteCinemaLogic;
        private readonly CreateRoomTypeLogic _createRoomTypeLogic;
        private readonly GetAllRoomTypeLogic _getAllRoomTypeLogic;
        private readonly UpdateRoomTypeLogic _updateRoomTypeLogic;
        private readonly DeleteRoomTypeLogic _deleteRoomTypeLogic;
        private readonly GetAllSeatTypeLogic _getAllSeatTypeLogic;
        private readonly CreateSeatTypeLogic _createSeatTypeLogic;
        private readonly UpdateSeatTypeLogic _updateSeatTypeLogic;
        private readonly DeleteSeatTypeLogic _deleteSeatTypeLogic;
        private readonly CreateRoomsLogic _createRoomsLogic;
        private readonly GetAllRoomLogic _getAllRoomLogic;
        private readonly UpdateRoomLogic _updateRoomLogic;
        private readonly DeleteRoomLogic _deleteRoomLogic;
        private readonly GetAllSeatLogic _getAllSeatLogic;
        private readonly UpdateSeatLogic _updateSeatLogic;
        private readonly GetShowtimesByMovieLogic _getShowtimesByMovieLogic;
        private readonly GetShowtimesByCinemaLogic _getShowtimesByCinemaLogic;
        private readonly GetShowtimeDetailsLogic _getShowtimeDetailsLogic;
        private readonly CreateShowtimeLogic _createShowtimeLogic;
        private readonly UpdateShowtimeLogic _updateShowtimeLogic;
        private readonly GetShowtimeSeatsLogic _getShowtimeSeatsLogic;
        private readonly GetBookingLogic _getBookingLogic;
        private readonly CreateBookingLogic _createBookingLogic;
        private readonly UpdateBookingLogic _updateBookingLogic;
        private readonly GetAllFoodDrinkLogic _getAllFoodDrinkLogic;
        private readonly CreateFoodDrinkLogic _createFoodDrinkLogic;
        private readonly UpdateFoodDrinkLogic _updateFoodDrinkLogic;
        private readonly DeleteFoodDrinkLogic _deleteFoodDrinkLogic;
        private readonly CheckInBookingLogic _checkInBookingLogic;
        private readonly GetBookingHistoryLogic _getBookingHistoryLogic;
        public CinemaGrpcServiceImpl(IMapper mapper, 
                                    CreateCinemaLogic createCinemaLogic, 
                                    GetAllCinemaLogic getAllCinemaLogic, 
                                    UpdateCinemaLogic updateCinemaLogic,
                                    DeleteCinemaLogic deleteCinemaLogic,
                                    CreateRoomTypeLogic createRoomTypeLogic,
                                    GetAllRoomTypeLogic getAllRoomTypeLogic,
                                    UpdateRoomTypeLogic updateRoomTypeLogic,
                                    DeleteRoomTypeLogic deleteRoomTypeLogic,
                                    GetAllSeatTypeLogic getAllSeatTypeLogic,
                                    CreateSeatTypeLogic createSeatTypeLogic,
                                    UpdateSeatTypeLogic updateSeatTypeLogic,
                                    DeleteSeatTypeLogic deleteSeatTypeLogic,
                                    CreateRoomsLogic createRoomsLogic,
                                    GetAllRoomLogic getAllRoomLogic,
                                    UpdateRoomLogic updateRoomLogic,
                                    DeleteRoomLogic deleteRoomLogic,
                                    GetAllSeatLogic getAllSeatLogic,
                                    UpdateSeatLogic updateSeatLogic,
                                    GetShowtimesByMovieLogic getShowtimesByMovieLogic,
                                    GetShowtimesByCinemaLogic getShowtimesByCinemaLogic,
                                    CreateShowtimeLogic createShowtimeLogic,
                                    UpdateShowtimeLogic updateShowtimeLogic,
                                    GetShowtimeSeatsLogic getShowtimeSeatsLogic,
                                    GetShowtimeDetailsLogic getShowtimeDetailsLogic,
                                    GetBookingLogic getBookingLogic,
                                    CreateBookingLogic createBookingLogic,
                                    UpdateBookingLogic updateBookingLogic,
                                    GetAllFoodDrinkLogic getAllFoodDrinkLogic,
                                    CreateFoodDrinkLogic createFoodDrinkLogic,
                                    UpdateFoodDrinkLogic updateFoodDrinkLogic,
                                    DeleteFoodDrinkLogic deleteFoodDrinkLogic,
                                    CheckInBookingLogic checkInBookingLogic,
                                    GetBookingHistoryLogic getBookingHistoryLogic) 
        {
            _mapper = mapper;
            _createCinemaLogic = createCinemaLogic;
            _getAllCinemaLogic = getAllCinemaLogic;
            _updateCinemaLogic = updateCinemaLogic;
            _deleteCinemaLogic = deleteCinemaLogic;
            _createRoomTypeLogic = createRoomTypeLogic;
            _getAllRoomTypeLogic = getAllRoomTypeLogic;
            _updateRoomTypeLogic = updateRoomTypeLogic;
            _deleteRoomTypeLogic = deleteRoomTypeLogic;
            _getAllSeatTypeLogic = getAllSeatTypeLogic;
            _createSeatTypeLogic = createSeatTypeLogic;
            _updateSeatTypeLogic = updateSeatTypeLogic;
            _deleteSeatTypeLogic = deleteSeatTypeLogic;
            _createRoomsLogic = createRoomsLogic;
            _getAllRoomLogic = getAllRoomLogic;
            _updateRoomLogic = updateRoomLogic;
            _deleteRoomLogic = deleteRoomLogic;
            _getAllSeatLogic = getAllSeatLogic;
            _updateSeatLogic = updateSeatLogic;
            _getShowtimesByMovieLogic = getShowtimesByMovieLogic;
            _getShowtimesByCinemaLogic = getShowtimesByCinemaLogic;
            _getShowtimeDetailsLogic = getShowtimeDetailsLogic;
            _createShowtimeLogic = createShowtimeLogic;
            _updateShowtimeLogic = updateShowtimeLogic;
            _getShowtimeSeatsLogic = getShowtimeSeatsLogic;
            _getBookingLogic = getBookingLogic;
            _createBookingLogic = createBookingLogic;
            _updateBookingLogic = updateBookingLogic;
            _updateBookingLogic = updateBookingLogic;
            _getAllFoodDrinkLogic = getAllFoodDrinkLogic;
            _createFoodDrinkLogic = createFoodDrinkLogic;
            _updateFoodDrinkLogic = updateFoodDrinkLogic;
            _deleteFoodDrinkLogic = deleteFoodDrinkLogic;
            _checkInBookingLogic = checkInBookingLogic;
            _getBookingHistoryLogic = getBookingHistoryLogic;
        }

        public override async Task<GetAllCinemasGrpcReplyDTO> GetAllCinemas(GetAllCinemasGrpcRequestDTO request, ServerCallContext context)
        {
            Guid? cinemaId = null;
            if (!string.IsNullOrWhiteSpace(request.Id)
                && Guid.TryParse(request.Id, out var parsedId))
            {
                cinemaId = parsedId;
            }

            var result = await _getAllCinemaLogic.Execute(new GetAllCinemasParam
            {
                Id = cinemaId,
                Name = request.Name,
                City = request.City,
                Status = request.Status,
            });

            return _mapper.Map<GetAllCinemasGrpcReplyDTO>(result);
        }

        public override async Task<CreateCinemaGrpcReplyDTO> CreateCinema(CreateCinemaGrpcRequestDTO request, ServerCallContext context)
        {
            try
            {
                var result = await _createCinemaLogic.Execute(new CreateCinemaParam
                {
                    Name = request.Name,
                    Address = request.Address,
                    City = request.City,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    OpenTime = TimeOnly.Parse(request.OpenTime),
                    CloseTime = TimeOnly.Parse(request.CloseTime),
                    Status = string.IsNullOrWhiteSpace(request.Status) ? "Active" : request.Status,
                    CreateBy = string.IsNullOrWhiteSpace(request.CreatedBy) ? "System" : request.CreatedBy
                });

                return _mapper.Map<CreateCinemaGrpcReplyDTO>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 [GRPC ERROR] {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw; // để gRPC biết có lỗi
            }
        }

        public override async Task<UpdateCinemaGrpcReplyDTO> UpdateCinema(UpdateCinemaGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _updateCinemaLogic.Execute(new UpdateCinemaParam
            {
                Id = Guid.Parse(request.Id),
                Name = request.Name,
                Address = request.Address,
                City = request.City,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                OpenTime = TimeOnly.Parse(request.OpenTime),
                CloseTime = TimeOnly.Parse(request.CloseTime),
                Status = request.Status,
            });

            return _mapper.Map<UpdateCinemaGrpcReplyDTO>(result);
        }

        public override async Task<DeleteCinemaGrpcReplyDTO> DeleteCinema(DeleteCinemaGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _deleteCinemaLogic.Execute(new DeleteCinemaParam
            {
                Id = Guid.Parse(request.Id),
            });

            return _mapper.Map<DeleteCinemaGrpcReplyDTO>(result);
        }


        public override async Task<GetAllRoomTypesGrpcReplyDTO> GetAllRoomTypes(GetAllRoomTypesGrpcRequestDTO request, ServerCallContext context)
        {
            Guid? roomTypeId = null;
            if (!string.IsNullOrWhiteSpace(request.Id)
                && Guid.TryParse(request.Id, out var parsedId))
            {
                roomTypeId = parsedId;
            }

            decimal? basePrice = null;
            if (!string.IsNullOrWhiteSpace(request.BasePrice)
                && decimal.TryParse(request.BasePrice, out var parsedPrice))
            {
                basePrice = parsedPrice;
            }

            var result = await _getAllRoomTypeLogic.Execute(new GetAllRoomTypeParam
            {
                Id = roomTypeId,
                Type = request.Type,
                BasePrice = basePrice,
            });

            return _mapper.Map<GetAllRoomTypesGrpcReplyDTO>(result);
        }

        public override async Task<CreateRoomTypeGrpcReplyDTO> CreateRoomType(CreateRoomTypeGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _createRoomTypeLogic.Execute(new CreateRoomTypeParam
            {
                Type = request.Type,
                BasePrice = decimal.Parse(request.BasePrice),
                CreatedBy = string.IsNullOrWhiteSpace(request.CreatedBy) ? "System" : request.CreatedBy
            });

            return _mapper.Map<CreateRoomTypeGrpcReplyDTO>(result);
        }

        public override async Task<UpdateRoomTypeGrpcReplyDTO> UpdateRoomType(UpdateRoomTypeGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _updateRoomTypeLogic.Execute(new UpdateRoomTypeParam
            {
                Id = Guid.Parse(request.Id),
                Type = request.Type,
                BasePrice = decimal.Parse(request.BasePrice),
            });

            return _mapper.Map<UpdateRoomTypeGrpcReplyDTO>(result);
        }

        public override async Task<DeleteRoomTypeGrpcReplyDTO> DeleteRoomType(DeleteRoomTypeGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _deleteRoomTypeLogic.Execute(new DeleteRoomTypeParam
            {
                Id = Guid.Parse(request.Id),
            });

            return _mapper.Map<DeleteRoomTypeGrpcReplyDTO>(result);
        }

        public override async Task<GetAllSeatTypesGrpcReplyDTO> GetAllSeatTypes(GetAllSeatTypesGrpcRequestDTO request, ServerCallContext context)
        {
            Guid? seatTypeId = null;
            if (!string.IsNullOrWhiteSpace(request.Id)
                && Guid.TryParse(request.Id, out var parsedId))
            {
                seatTypeId = parsedId;
            }

            decimal? extraPrice = null;
            if (!string.IsNullOrWhiteSpace(request.ExtraPrice)
                && decimal.TryParse(request.ExtraPrice, out var parsedPrice))
            {
                extraPrice = parsedPrice;
            }

            var result = await _getAllSeatTypeLogic.Execute(new GetAllSeatTypeParam
            {
                Id = seatTypeId,
                Type = request.Type,
                ExtraPrice = extraPrice,
            });

            return _mapper.Map<GetAllSeatTypesGrpcReplyDTO>(result);
        }

        public override async Task<CreateSeatTypeGrpcReplyDTO> CreateSeatType(CreateSeatTypeGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _createSeatTypeLogic.Execute(new CreateSeatTypeParam
            {
                Type = request.Type,
                ExtraPrice = decimal.Parse(request.ExtraPrice),
                CreatedBy = string.IsNullOrWhiteSpace(request.CreatedBy) ? "System" : request.CreatedBy
            });

            return _mapper.Map<CreateSeatTypeGrpcReplyDTO>(result);
        }

        public override async Task<UpdateSeatTypeGrpcReplyDTO> UpdateSeatType(UpdateSeatTypeGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _updateSeatTypeLogic.Execute(new UpdateSeatTypeParam
            {
                Id = Guid.Parse(request.Id),
                Type = request.Type,
                ExtraPrice = decimal.Parse(request.ExtraPrice),
            });

            return _mapper.Map<UpdateSeatTypeGrpcReplyDTO>(result);
        }

        public override async Task<DeleteSeatTypeGrpcReplyDTO> DeleteSeatType(DeleteSeatTypeGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _deleteSeatTypeLogic.Execute(new DeleteSeatTypeParam
            {
                Id = Guid.Parse(request.Id),
            });

            return _mapper.Map<DeleteSeatTypeGrpcReplyDTO>(result);
        }

        public override async Task<CreateRoomGrpcReplyDTO> CreateRoom(CreateRoomGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _createRoomsLogic.Execute(new CreateRoomParam
            {
                RoomNumber = request.RoomNumber,
                Status = request.Status,
                Total_Column = request.TotalColumn,
                Total_Row = request.TotalRow,
                RoomTypeId = Guid.Parse(request.RoomTypeId),
                CinemaId = Guid.Parse(request.CinemaId),
                CreatedBy = string.IsNullOrWhiteSpace(request.CreatedBy) ? "System" : request.CreatedBy
            });

            return _mapper.Map<CreateRoomGrpcReplyDTO>(result);
        }

        public override async Task<GetAllRoomsGrpcReplyDTO> GetAllRooms(GetAllRoomsGrpcRequestDTO request, ServerCallContext context)
        {
            Guid? roomId = null;
            if (!string.IsNullOrWhiteSpace(request.Id)
                && Guid.TryParse(request.Id, out var parsedId))
            {
                roomId = parsedId;
            }

            var result = await _getAllRoomLogic.Execute(new GetAllRoomParam
            {
                CinemaId = Guid.Parse(request.CinemaId),
                Id = roomId,
                RoomNumber = request.RoomNumber,
                Status = request.Status,
                Type = request.Type,
            });

            return _mapper.Map<GetAllRoomsGrpcReplyDTO>(result);
        }

        public override async Task<UpdateRoomGrpcReplyDTO> UpdateRoom(UpdateRoomGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _updateRoomLogic.Execute(new UpdateRoomParam
            {
                Id = Guid.Parse(request.Id),
                RoomNumber = request.RoomNumber,
                Total_Column = request.TotalColumn,
                Total_Row = request.TotalRow,
                Status = request.Status,
                RoomTypeId = Guid.Parse(request.RoomTypeId),
                CinemaId = Guid.Parse(request.CinemaId)
            });

            return _mapper.Map<UpdateRoomGrpcReplyDTO>(result);
        }

        public override async Task<DeleteRoomGrpcReplyDTO> DeleteRoom(DeleteRoomGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _deleteRoomLogic.Execute(new DeleteRoomParam
            {
                Id = Guid.Parse(request.Id),
            });

            return _mapper.Map<DeleteRoomGrpcReplyDTO>(result);
        }

        public override async Task<GetAllSeatsGrpcReplyDTO> GetAllSeats(GetAllSeatsGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _getAllSeatLogic.Execute(new GetAllSeatParam
            {
                RoomId = Guid.Parse(request.RoomId)
            });

            return _mapper.Map<GetAllSeatsGrpcReplyDTO>(result);
        }

        public override async Task<UpdateSeatsGrpcReplyDTO> UpdateSeats(UpdateSeatsGrpcRequestDTO request, ServerCallContext context)
        {
            var seatIds = new List<Guid>();

            if (request.Id != null && request.Id.Count > 0)
            {
                seatIds = request.Id
                    .Where(id => Guid.TryParse(id, out _))
                    .Select(Guid.Parse)
                    .ToList();
            }

            Guid? seatTypeId = null;
            if (!string.IsNullOrWhiteSpace(request.SeatTypeId) 
                && Guid.TryParse(request.SeatTypeId, out var parsedSeatTypeId))
            {
                seatTypeId = parsedSeatTypeId;
            }

            var result = await _updateSeatLogic.Execute(new UpdateSeatParam
            {
                Id = seatIds,
                isActive = request.IsActive,
                SeatTypeId = seatTypeId
            });

            return _mapper.Map<UpdateSeatsGrpcReplyDTO>(result);
        }

        public override async Task<GetShowtimesGrpcReplyDTO> GetShowtimes(GetShowtimesGrpcRequestDTO request, ServerCallContext context)
        {
            Guid? id = null;
            if (!string.IsNullOrWhiteSpace(request.Id)
                && Guid.TryParse(request.Id, out var parsedId))
            {
                id = parsedId;
            }

            Console.WriteLine(JsonSerializer.Serialize(request));

            var result = await _getShowtimesByMovieLogic.Execute(new GetShowtimesByMovieParam
            {
                Id = id,
                MovieId = Guid.Parse(request.MovieId),
                Country = request.Country,
                Date = DateOnly.Parse(request.Date),
            });

            return _mapper.Map<GetShowtimesGrpcReplyDTO>(result);
        }

        public override async Task<GetShowtimesByCinemaGrpcReplyDTO> GetShowtimesByCinema(GetShowtimesByCinemaGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _getShowtimesByCinemaLogic.Execute(new GetShowtimesByCinemaParam
            {
                CinemaId = Guid.Parse(request.CinemaId),
                Date = DateOnly.Parse(request.Date)
            });

            return _mapper.Map<GetShowtimesByCinemaGrpcReplyDTO>(result);
        }

        public override async Task<GetShowtimeDetailsGrpcReplyDTO> GetShowtimeDetails(GetShowtimeDetailsGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _getShowtimeDetailsLogic.Execute(new GetShowtimeDetailsParam
            {
                ShowtimeId = Guid.Parse(request.ShowtimeId)
            });

            return _mapper.Map<GetShowtimeDetailsGrpcReplyDTO>(result);
        }

        public override async Task<CreateShowtimeGrpcReplyDTO> CreateShowtime(CreateShowtimeGrpcRequestDTO request, ServerCallContext context)
        {
            Console.WriteLine(JsonSerializer.Serialize(request));

            // Parse datetime, handle AM/PM input, convert sang UTC
            DateTime startTime = DateTime.Parse(request.StartTime, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            DateTime endTime = DateTime.Parse(request.EndTime, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);

            // Nếu EndTime <= StartTime, nghĩa là qua nửa đêm
            if (endTime <= startTime)
                endTime = endTime.AddDays(1);

            var result = await _createShowtimeLogic.Execute(new CreateShowtimeParam
            {
                MovieId = Guid.Parse(request.MovieId),
                RoomId = Guid.Parse(request.RoomId),
                StartTime = startTime.ToUniversalTime(),
                EndTime = endTime.ToUniversalTime(),
                Status = request.Status,
            });

            return _mapper.Map<CreateShowtimeGrpcReplyDTO>(result);
        }

        public override async Task<UpdateShowtimeGrpcReplyDTO> UpdateShowtime(UpdateShowtimeGrpcRequestDTO request, ServerCallContext context)
        {
            Console.WriteLine(JsonSerializer.Serialize(request));

            Guid? movieId = null;
            if (!string.IsNullOrWhiteSpace(request.Id)
                && Guid.TryParse(request.MovieId, out var movieParsedId))
            {
                movieId = movieParsedId;
            }

            Guid? roomId = null;
            if (!string.IsNullOrWhiteSpace(request.Id)
                && Guid.TryParse(request.RoomId, out var roomParsedId))
            {
                roomId = roomParsedId;
            }

            DateTime? startTime = null;
            DateTime? endTime = null;

            if (!string.IsNullOrEmpty(request.StartTime))
            {
                startTime = DateTime.Parse(request.StartTime, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);

                // Nếu EndTime <= StartTime, nghĩa là qua nửa đêm
                if (!string.IsNullOrEmpty(request.EndTime))
                {
                    endTime = DateTime.Parse(request.EndTime, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);

                    if (endTime <= startTime)
                        endTime = endTime.Value.AddDays(1);
                }

                // Convert sang UTC
                startTime = startTime.Value.ToUniversalTime();
                if (endTime.HasValue)
                    endTime = endTime.Value.ToUniversalTime();
            }
            else if (!string.IsNullOrEmpty(request.EndTime))
            {
                // Trường hợp chỉ có EndTime được truyền, vẫn parse
                endTime = DateTime.Parse(request.EndTime, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
                endTime = endTime.Value.ToUniversalTime();
            }


            var result = await _updateShowtimeLogic.Execute(new UpdateShowtimeParam
            {
                Id = Guid.Parse(request.Id),
                MovieId = movieId,
                RoomId = roomId,
                StartTime = startTime,
                EndTime = endTime,
                Status = request.Status,
            });

            return _mapper.Map<UpdateShowtimeGrpcReplyDTO>(result);
        } 

        public override async Task<GetShowtimeSeatsGrpcReplyDTO> GetShowtimeSeats(GetShowtimeSeatsGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _getShowtimeSeatsLogic.Execute(new GetShowtimeSeatsParam
            {
                showtimeId = Guid.Parse(request.Id)
            });

            return _mapper.Map<GetShowtimeSeatsGrpcReplyDTO>(result);
        }

        public override async Task<GetBookingGrpcReplyDTO> GetBooking(GetBookingGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _getBookingLogic.Execute(new GetBookingParam
            {
                BookingId = Guid.Parse(request.BookingId),
            });

            return _mapper.Map<GetBookingGrpcReplyDTO>(result);
        }

        public override async Task<CreateBookingGrpcReplyDTO> CreateBooking(CreateBookingGrpcRequestDTO request, ServerCallContext context)
        {
            var param = new CreateBookingParam
            {
                UserId = Guid.Parse(request.UserId),
                ShowtimeId = Guid.Parse(request.ShowtimeId),
                ShowtimeSeatIds = request.ShowtimeSeatIds.Select(s => Guid.Parse(s)).ToList(),
            };

            if (request.FoodDrinkItems != null && request.FoodDrinkItems.Count > 0)
            {
                param.FoodDrinkItems = request.FoodDrinkItems.Select(x => new CreateBookingFoodDrinkItemParam
                {
                    FoodDrinkId = Guid.Parse(x.FoodDrinkId),
                    Quantity = x.Quantity
                }).ToList();
            }

            var result = await _createBookingLogic.Execute(param);

            return _mapper.Map<CreateBookingGrpcReplyDTO>(result);
        }

        public override async Task<UpdateBookingGrpcReplyDTO> UpdateBooking(UpdateBookingGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _updateBookingLogic.Execute(new UpdateBookingParam
            {
                BookingId = Guid.Parse(request.BookingId),
                Status = request.Status
            });

            return _mapper.Map<UpdateBookingGrpcReplyDTO>(result);
        }

        public override async Task<GetAllFoodDrinksGrpcReplyDTO> GetAllFoodDrinks(GetAllFoodDrinksGrpcRequestDTO request, ServerCallContext context)
        {
            Guid? id = null;
            if (!string.IsNullOrWhiteSpace(request.Id) && Guid.TryParse(request.Id, out var parsedId))
            {
                id = parsedId;
            }

            var result = await _getAllFoodDrinkLogic.Execute(new GetAllFoodDrinkParam
            {
                Id = id,
                Name = request.Name,
                Type = request.Type,
                Size = request.Size,
            });

            return _mapper.Map<GetAllFoodDrinksGrpcReplyDTO>(result);
        }

        public override async Task<CreateFoodDrinkGrpcReplyDTO> CreateFoodDrink(CreateFoodDrinkGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _createFoodDrinkLogic.Execute(new CreateFoodDrinkParam
            {
                Name = request.Name,
                Type = request.Type,
                Size = request.Size,
                Price = decimal.Parse(request.Price),
                CreatedBy = request.CreatedBy
            });

            return _mapper.Map<CreateFoodDrinkGrpcReplyDTO>(result);
        }

        public override async Task<UpdateFoodDrinkGrpcReplyDTO> UpdateFoodDrink(UpdateFoodDrinkGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _updateFoodDrinkLogic.Execute(new UpdateFoodDrinkParam
            {
                Id = Guid.Parse(request.Id),
                Name = request.Name,
                Type = request.Type,
                Size = request.Size,
                Price = string.IsNullOrWhiteSpace(request.Price) ? null : decimal.Parse(request.Price)
            });

            return _mapper.Map<UpdateFoodDrinkGrpcReplyDTO>(result);
        }

        public override async Task<DeleteFoodDrinkGrpcReplyDTO> DeleteFoodDrink(DeleteFoodDrinkGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _deleteFoodDrinkLogic.Execute(new DeleteFoodDrinkParam
            {
                Id = Guid.Parse(request.Id),
            });

            return _mapper.Map<DeleteFoodDrinkGrpcReplyDTO>(result);
        }

        public override async Task<CheckInBookingGrpcReplyDTO> CheckInBooking(CheckInBookingGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _checkInBookingLogic.Execute(new CheckInBookingParam
            {
                BookingId = Guid.Parse(request.BookingId),
                StaffUserId = Guid.Parse(request.StaffUserId)
            });

            return _mapper.Map<CheckInBookingGrpcReplyDTO>(result);
        }

        public override async Task<GetBookingHistoryGrpcReplyDTO> GetBookingHistory(GetBookingHistoryGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _getBookingHistoryLogic.Execute(new GetBookingHistoryParam
            {
                UserId = Guid.Parse(request.UserId)
            });

            return _mapper.Map<GetBookingHistoryGrpcReplyDTO>(result);
        }
    }
}
