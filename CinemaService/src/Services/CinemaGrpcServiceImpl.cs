using AutoMapper;
using CinemaGrpc;
using Grpc.Core;
using src.DataTransferObject.Parameter;
using src.DomainLogic;

namespace src.Services
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
                                    DeleteSeatTypeLogic deleteSeatTypeLogic) 
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
    }
}
