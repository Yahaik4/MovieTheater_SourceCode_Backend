﻿using AuthenticationGrpc;
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
        private readonly RefreshTokenLogic _refreshTokenLogic;
        private readonly LogoutLogic _logoutLogic;

        public AuthenticationGrpcServiceImpl(IMapper mapper, LoginLogic loginLogic, RefreshTokenLogic refreshTokenLogic, LogoutLogic logoutLogic) 
        {
            _mapper = mapper;
            _loginLogic = loginLogic;
            _refreshTokenLogic = refreshTokenLogic;
            _logoutLogic = logoutLogic;
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
    }
}
