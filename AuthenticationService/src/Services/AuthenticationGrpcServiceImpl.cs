using AuthenticationGrpc;
using AutoMapper;
using Grpc.Core;
using src.DataTransferObject.Parameter;
using src.DomainLogic;

namespace src.Services
{
    public class AuthenticationGrpcServiceImpl : AuthenticationGrpcService.AuthenticationGrpcServiceBase
    {
        private readonly IMapper _mapper;
        private readonly LoginLogic _loginLogic;

        public AuthenticationGrpcServiceImpl(IMapper mapper, LoginLogic loginLogic) 
        {
            _mapper = mapper;
            _loginLogic = loginLogic;
        }

        public override async Task<LoginGprcReplyDTO> Login (LoginGprcRequestDTO request, ServerCallContext context)
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
            if (result.Result)
            {
                var httpContext = context.GetHttpContext();
                httpContext.Response.Cookies.Append("access_token", result.Data.AccessToken, new CookieOptions { HttpOnly = true });
                httpContext.Response.Cookies.Append("refresh_token", result.Data.RefreshToken, new CookieOptions { HttpOnly = true });
            }

            return _mapper.Map<LoginGprcReplyDTO>(result);
        }
    }
}
