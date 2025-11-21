using AuthenticationService.DataTransferObject.Parameter;
using AuthenticationService.DataTransferObject.ResultData;
using AuthenticationService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace AuthenticationService.DomainLogic
{
    public class VerifyAccountLogic : IDomainLogic<VerifyAccountParam, Task<VerifyAccountResultData>>
    {
        private readonly IUserRepository _userRepository;

        public VerifyAccountLogic(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<VerifyAccountResultData> Execute(VerifyAccountParam param)
        {
            if (param.UserId == Guid.Empty)
            {
                throw new ValidationException("UserId is required");
            }

            var user = await _userRepository.GetUserById(param.UserId);

            if (user == null || user.IsDeleted)
            {
                throw new NotFoundException("User not found");
            }

            if (user.IsVerified)
            {
                return new VerifyAccountResultData
                {
                    Result = true,
                    Message = "Account already verified",
                    StatusCode = StatusCodeEnum.Success
                };
            }

            user.IsVerified = true;
            user.UpdatedAt = DateTime.UtcNow;
            user.IsDeleted = false;

            await _userRepository.UpdateUser(user);

            return new VerifyAccountResultData
            {
                Result = true,
                Message = "Account verified successfully",
                StatusCode = StatusCodeEnum.Success
            };
        }
    }
}

