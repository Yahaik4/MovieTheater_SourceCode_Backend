using AutoMapper;
using CinemaGrpc;
using Grpc.Core;
using src.DataTransferObject.Parameter;
using src.DomainLogic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace src.Services
{
    public class CinemaGrpcServiceImpl : CinemaGrpcService.CinemaGrpcServiceBase
    {
        private readonly IMapper _mapper;
        private readonly CreateCinemaLogic _createCinemaLogic;
        private readonly GetAllCinemaLogic _getAllCinemaLogic;
        private readonly UpdateCinemaLogic _updateCinemaLogic;
        private readonly DeleteCinemaLogic _deleteCinemaLogic;

        public CinemaGrpcServiceImpl(IMapper mapper, 
                                    CreateCinemaLogic createCinemaLogic, 
                                    GetAllCinemaLogic getAllCinemaLogic, 
                                    UpdateCinemaLogic updateCinemaLogic,
                                    DeleteCinemaLogic deleteCinemaLogic) 
        {
            _mapper = mapper;
            _createCinemaLogic = createCinemaLogic;
            _getAllCinemaLogic = getAllCinemaLogic;
            _updateCinemaLogic = updateCinemaLogic;
            _deleteCinemaLogic = deleteCinemaLogic;
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
                    Status = request.Status,
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
    }
}
