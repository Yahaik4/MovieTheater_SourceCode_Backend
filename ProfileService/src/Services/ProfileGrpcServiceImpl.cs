using AutoMapper;
using Grpc.Core;
using ProfileGrpc;
using ProfileService.DomainLogic;
using ProfileService.DataTransferObject.Parameter;

namespace ProfileService.Services
{
    public class ProfileGrpcServiceImpl : ProfileGrpcService.ProfileGrpcServiceBase
    {
        private readonly IMapper _mapper;
        private readonly CreateProfileLogic _createProfileLogic;

        public ProfileGrpcServiceImpl(IMapper mapper, CreateProfileLogic createProfileLogic) 
        {
            _mapper = mapper;
            _createProfileLogic = createProfileLogic;
        }

        public override async Task<CreateProfileGrpcReplyDTO> CreateProfile(CreateProfileGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _createProfileLogic.Execute(new CreateProfileParam
            {
                FullName = request.FullName,
                Role = request.Role,
                UserId = Guid.Parse(request.UserId),
            });

            return _mapper.Map<CreateProfileGrpcReplyDTO>(result);
        }
    }
}
