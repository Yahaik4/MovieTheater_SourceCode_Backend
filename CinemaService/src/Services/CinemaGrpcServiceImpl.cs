using CinemaGrpc;
using AutoMapper;
using Grpc.Core;
using src.DataTransferObject.Parameter;
using src.DomainLogic;

namespace src.Services
{
    public class CinemaGrpcServiceImpl : CinemaGrpcService.CinemaGrpcServiceBase
    {
        private readonly IMapper _mapper;
        private readonly CreateCinemaLogic _createCinemaLogic;

        public CinemaGrpcServiceImpl(IMapper mapper, CreateCinemaLogic createCinemaLogic) 
        {
            _mapper = mapper;
            _createCinemaLogic = createCinemaLogic;
        }

        public override async Task<CreateCinemaGrpcReplyDTO> CreateCinema(CreateCinemaGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _createCinemaLogic.Execute(new CreateCinemaParam
            {
                Name = request.Name,
                Address = request.Address,
                City = request.City,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Open_Time = TimeOnly.Parse(request.OpenTime),
                Close_Time = TimeOnly.Parse(request.CloseTime),
                Status = request.Status,
            });

            return _mapper.Map<CreateCinemaGrpcReplyDTO>(result);
        }
    }
}
