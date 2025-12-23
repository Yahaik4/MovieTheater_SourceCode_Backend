using AuthenticationGrpc;
using AuthenticationService.DataTransferObject.Parameter;
using AuthenticationService.DomainLogic;
using AutoMapper;
using Grpc.Core;
using System.Net;

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
        private readonly AddUserLogic _addUserLogic;
        private readonly GetCustomersLogic _getCustomersLogic;
        private readonly GetStaffsLogic _getStaffsLogic;
        private readonly DeleteUserLogic _deleteUserLogic;
        private readonly UpdateCustomerLogic _updateCustomerLogic;
        private readonly UpdateStaffLogic _updateStaffLogic;
        private readonly GetEmailLogic _getEmailLogic;
        public AuthenticationGrpcServiceImpl(IMapper mapper, 
                                            LoginLogic loginLogic, 
                                            RefreshTokenLogic refreshTokenLogic, 
                                            LogoutLogic logoutLogic, 
                                            RegisterLogic registerLogic,
                                            VerifyAccountLogic verifyAccountLogic,
                                            ResendOtpLogic resendOtpLogic,
                                            ResetPasswordLogic resetPasswordLogic,
                                            AddUserLogic addUserLogic,
                                            GetCustomersLogic getCustomersLogic,
                                            GetStaffsLogic getStaffsLogic,
                                            DeleteUserLogic deleteUserLogic,
                                            UpdateCustomerLogic updateCustomerLogic,
                                            UpdateStaffLogic updateStaffLogic,
                                            GetEmailLogic getEmailLogic
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
            _addUserLogic = addUserLogic;
            _getCustomersLogic = getCustomersLogic;
            _getStaffsLogic = getStaffsLogic;
            _deleteUserLogic = deleteUserLogic;
            _updateCustomerLogic = updateCustomerLogic;
            _updateStaffLogic = updateStaffLogic;
            _getEmailLogic = getEmailLogic;
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

        public override async Task<RegisterGrpcReplyDTO> AddUser(AddUserGrpcRequestDTO request, ServerCallContext context)
        {
            var param = new AddUserParam
            {
                FullName = request.FullName,
                Email = request.Email,
                Password = request.Password,
                Role = request.Role,
                PhoneNumber = request.PhoneNumber,
                Gender = request.Gender,
                Position = request.Position,
                Salary = string.IsNullOrWhiteSpace(request.Salary)
                ? null
                : decimal.TryParse(request.Salary, out var sal) ? sal : null
            };

            if (!string.IsNullOrEmpty(request.CinemaId))
            {
                param.CinemaId = Guid.Parse(request.CinemaId);
            }

            if (!string.IsNullOrEmpty(request.DayOfBirth))
            {
                if (DateTime.TryParse(request.DayOfBirth, out var dob))
                {
                    param.DayOfBirth = dob;
                }
            }

            var result = await _addUserLogic.Execute(param);
            return _mapper.Map<RegisterGrpcReplyDTO>(result);
        }

        public override async Task<GetStaffsGrpcReplyDTO> GetStaffs(
            GetUsersGrpcRequestDTO request,
            ServerCallContext context)
        {
            Guid? userId = null;
            if (!string.IsNullOrWhiteSpace(request.UserId)
                && Guid.TryParse(request.UserId, out var parsedUserId))
            {
                userId = parsedUserId;
            }

            Guid? cinemaId = null;
            if (!string.IsNullOrWhiteSpace(request.CinemaId)
                && Guid.TryParse(request.CinemaId, out var parsedCinemaId))
            {
                cinemaId = parsedCinemaId;
            }

            var result = await _getStaffsLogic.Execute(new GetUsersParam
            {
                UserId = userId,
                CinemaId = cinemaId
            });

            return _mapper.Map<GetStaffsGrpcReplyDTO>(result);
        }

        public override async Task<GetCustomersGrpcReplyDTO> GetCustomers(GetUsersGrpcRequestDTO request, ServerCallContext context)
        {
            var param = new GetUsersParam();

            if (!string.IsNullOrWhiteSpace(request.UserId) &&
                Guid.TryParse(request.UserId, out var userId))
            {
                param.UserId = userId;
            }

            var result = await _getCustomersLogic.Execute(param);
            return _mapper.Map<GetCustomersGrpcReplyDTO>(result);
        }

        public override async Task<DeleteUserGrpcReplyDTO> DeleteUser(
            DeleteUserGrpcRequestDTO request,
            ServerCallContext context)
        {
            var param = new DeleteUserParam
            {
                TargetUserId = Guid.Parse(request.TargetUserId),
                CallerRole = request.CallerRole,
                CallerPosition = string.IsNullOrWhiteSpace(request.CallerPosition)
                    ? null
                    : request.CallerPosition
            };

            var result = await _deleteUserLogic.Execute(param);
            return _mapper.Map<DeleteUserGrpcReplyDTO>(result);
        }

        public override async Task<UpdateCustomerGrpcReplyDTO> UpdateCustomer(
            UpdateCustomerGrpcRequestDTO request,
            ServerCallContext context)
        {
            var param = new UpdateCustomerParam
            {
                TargetUserId = Guid.Parse(request.TargetUserId),
                CallerRole = request.CallerRole,
                CallerPosition = string.IsNullOrWhiteSpace(request.CallerPosition) ? null : request.CallerPosition,
                FullName = string.IsNullOrWhiteSpace(request.FullName) ? null : request.FullName,
                PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? null : request.PhoneNumber,
                DayOfBirth = string.IsNullOrWhiteSpace(request.DayOfBirth) ? null : request.DayOfBirth,
                Gender = string.IsNullOrWhiteSpace(request.Gender) ? null : request.Gender,
                Points = string.IsNullOrWhiteSpace(request.Points)
                    ? null
                    : int.TryParse(request.Points, out var pts) ? pts : null
            };

            var result = await _updateCustomerLogic.Execute(param);
            return _mapper.Map<UpdateCustomerGrpcReplyDTO>(result);
        }

        public override async Task<UpdateStaffGrpcReplyDTO> UpdateStaff(
            UpdateStaffGrpcRequestDTO request,
            ServerCallContext context)
        {
            var param = new UpdateStaffParam
            {
                TargetUserId = Guid.Parse(request.TargetUserId),
                CallerRole = request.CallerRole,
                CallerPosition = string.IsNullOrWhiteSpace(request.CallerPosition) ? null : request.CallerPosition,
                FullName = string.IsNullOrWhiteSpace(request.FullName) ? null : request.FullName,
                PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? null : request.PhoneNumber,
                DayOfBirth = string.IsNullOrWhiteSpace(request.DayOfBirth) ? null : request.DayOfBirth,
                Gender = string.IsNullOrWhiteSpace(request.Gender) ? null : request.Gender,
                CinemaId = string.IsNullOrWhiteSpace(request.CinemaId)
                    ? null
                    : Guid.TryParse(request.CinemaId, out var cid) ? cid : null,
                Position = string.IsNullOrWhiteSpace(request.Position) ? null : request.Position,
                Salary = string.IsNullOrWhiteSpace(request.Salary)
                    ? null
                    : decimal.TryParse(request.Salary, out var sal) ? sal : null
            };

            var result = await _updateStaffLogic.Execute(param);
            return _mapper.Map<UpdateStaffGrpcReplyDTO>(result);
        }

        public async override Task<GetEmailGrpcReplyDTO> GetEmail(GetEmailGrpcRequestDTO request, ServerCallContext context)
        {
            var result = await _getEmailLogic.Execute(new GetEmailParam
            {
                UserId = Guid.Parse(request.UserId)
            });

            return _mapper.Map<GetEmailGrpcReplyDTO>(result);
        }
    }
}
