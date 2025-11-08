using OTPGrpc;
using AutoMapper;
using src.DomainLogic;
using Grpc.Core;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
public class OtpGrpcServiceImpl : OtpGrpcService.OtpGrpcServiceBase
{
    private readonly IMapper _mapper;

    private readonly ValidateOtpLogic _validateOtpLogic;
    private readonly SendOtpLogic _sendOtpLogic;
    public OtpGrpcServiceImpl(IMapper mapper, SendOtpLogic sendOtpLogic, ValidateOtpLogic validateOtpLogic)
    {
        _mapper = mapper;
        _sendOtpLogic = sendOtpLogic;
        _validateOtpLogic = validateOtpLogic;
    }

    public override async Task<GenerateOtpResultGrpc> GenerateOTP(GenerateOtpRequestGrpc request, ServerCallContext context)
    {
        if (string.IsNullOrEmpty(request.Email))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Email is required."));

        var logicResult = await _sendOtpLogic.Execute(new GenerateOtpRequest
        {
            Email = request.Email
        });

        return _mapper.Map<GenerateOtpResultGrpc>(logicResult);
    }

    public override async Task<ValidateOtpResultGrpc> ValidateOTP(ValidateOtpRequestGrpc request, ServerCallContext context)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Otp))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Email and OTP are required."));

        var validateParam = new ValidateOtpRequest { Email = request.Email, Otp = request.Otp};
        var result = await _validateOtpLogic.Execute(validateParam);

        return _mapper.Map<ValidateOtpResultGrpc>(result);
    }

}