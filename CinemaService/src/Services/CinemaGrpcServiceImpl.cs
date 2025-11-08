using AutoMapper;
using CinemaGrpc;
using CinemaService.DomainLogic;
using Grpc.Core;
using CinemaService.DataTransferObject.Parameter;

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
                                    UpdateSeatLogic updateSeatLogic) 
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

    }
}
