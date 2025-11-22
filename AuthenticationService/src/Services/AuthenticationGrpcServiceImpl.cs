using AuthenticationGrpc;
using AuthenticationService.DomainLogic;
using AutoMapper;
using Grpc.Core;
using AuthenticationService.DataTransferObject.Parameter;

namespace AuthenticationService.Services
{
    public class AuthenticationGrpcServiceImpl : AuthenticationGrpcService.AuthenticationGrpcServiceBase
    {
        private readonly IMapper _mapper;
        private readonly LoginLogic _loginLogic;
        private readonly RefreshTokenLogic _refreshTokenLogic;
        private readonly LogoutLogic _logoutLogic;
        private readonly RegisterLogic _registerLogic;
        private readonly VerifyAccountLogic _verifyAccountLogic;
        private readonly ResendOtpLogic _resendOtpLogic;
        private readonly ResetPasswordLogic _resetPasswordLogic;
        public AuthenticationGrpcServiceImpl(IMapper mapper, 
                                            LoginLogic loginLogic, 
                                            RefreshTokenLogic refreshTokenLogic, 
                                            LogoutLogic logoutLogic, 
                                            RegisterLogic registerLogic,
                                            VerifyAccountLogic verifyAccountLogic,
                                            ResendOtpLogic resendOtpLogic,
                                            ResetPasswordLogic resetPasswordLogic
                                            ) 
        {
            _mapper = mapper;
            _loginLogic = loginLogic;
            _refreshTokenLogic = refreshTokenLogic;
            _logoutLogic = logoutLogic;
            _registerLogic = registerLogic;
            _verifyAccountLogic = verifyAccountLogic;
            _resendOtpLogic = resendOtpLogic;
            _resetPasswordLogic = resetPasswordLogic;
        }

        public override async Task<LoginGrpcReplyDTO> Login(LoginGrpcRequestDTO request, ServerCallContext context)
        {
            var ipAddress = context.Peer;
            var userAgent = context.RequestHeaders.FirstOrDefault(x => x.Key == "user-agent")?.Value;

            var result = await _loginLogic.Execute(new LoginParam
            {
                Email = request.Email,
                Password = request.Password,
                IpAddress = ipAddress,
                Device = userAgent,
            });

            return _mapper.Map<LoginGrpcReplyDTO>(result);
        }

        public override async Task<RefreshTokenGrpcReplyDTO> RefreshToken(RefreshTokenGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _refreshTokenLogic.Execute(new RefreshTokenParam
            {
                RefreshToken = request.RefreshToken,
            });

            return _mapper.Map<RefreshTokenGrpcReplyDTO>(result);
        }

        public override async Task<LogoutGrpcReplyDTO> Logout(RefreshTokenGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _logoutLogic.Execute(new RefreshTokenParam
            {
                RefreshToken = request.RefreshToken,
            });

            return _mapper.Map<LogoutGrpcReplyDTO>(result);
        }

        public override async Task<RegisterGrpcReplyDTO> Register(RegisterGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _registerLogic.Execute(new RegisterParam
            {
                FullName = request.FullName,
                Email = request.Email,
                Password = request.Password,
            });

            return _mapper.Map<RegisterGrpcReplyDTO>(result);
        }

        public override async Task<VerifyAccountGrpcReplyDTO> VerifyAccount(VerifyAccountGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _verifyAccountLogic.Execute(new VerifyAccountParam
            {
                UserId = Guid.Parse(request.UserId)
            });

            return _mapper.Map<VerifyAccountGrpcReplyDTO>(result);
        }

        public override async Task<ResendOTPReply> ResendOTP(ResendOTPRequest request, ServerCallContext context)
        {
            var result = await _resendOtpLogic.Execute(request.Email, request.Purpose);
            return result;
        }

        public override async Task<ResetpassWordReply> VerifyResetPassword(ResetPasswordRequest request, ServerCallContext context)
        {
            var result = await _resetPasswordLogic.Execute(
                email: request.Email,
                otp: request.Otp,
                newPassword: request.NewPassword
            );

            return new ResetpassWordReply
            {
                Result = result.Result,
                Message = result.Message,
                StatusCode = result.StatusCode
            };
        }
    }
}
