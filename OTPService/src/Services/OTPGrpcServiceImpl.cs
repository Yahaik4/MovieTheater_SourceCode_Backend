using AutoMapper;
using Grpc.Core;
using OTPGrpc;
using OTPService.DataTransferObject.Parameter;
using OTPService.DomainLogic;
using System.Text.Json;

namespace OTPService.Services
{
    public class OTPGrpcServiceImpl : OTPGrpcService.OTPGrpcServiceBase
    {
        private readonly IMapper _mapper;
        private readonly CreateOTPLogic _createOTPLogic;
        private readonly VerifyOTPLogic _verifyOTPLogic;
        public OTPGrpcServiceImpl(IMapper mapper, CreateOTPLogic createOTPLogic, VerifyOTPLogic verifyOTPLogic)

        {
            _mapper = mapper;
            _createOTPLogic = createOTPLogic;
            _verifyOTPLogic = verifyOTPLogic;
        }

        public override async Task<CreateOTPGrpcReplyDTO> CreateOTP(CreateOTPGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _createOTPLogic.Execute(new CreateOTPParam
            {
                UserId = Guid.Parse(request.UserId),
            });

            return _mapper.Map<CreateOTPGrpcReplyDTO>(result);
        }

        public override async Task<VerifyOTPGrpcReplyDTO> VerifyOTP(VerifyOTPGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _verifyOTPLogic.Execute(new VerifyOTPParam
            {
                UserId = Guid.Parse(request.UserId),
                Code = request.Code
            });

            return _mapper.Map<VerifyOTPGrpcReplyDTO>(result);
        }
    }
}
