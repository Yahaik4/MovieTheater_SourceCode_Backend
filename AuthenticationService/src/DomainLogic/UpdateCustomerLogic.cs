using AuthenticationService.DataTransferObject.Parameter;
using AuthenticationService.DataTransferObject.ResultData;
using AuthenticationService.Infrastructure.Repositories.Interfaces;
using AuthenticationService.ServiceConnector.ProfileService;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace AuthenticationService.DomainLogic
{
    public class UpdateCustomerLogic : IDomainLogic<UpdateCustomerParam, Task<UpdateCustomerResultData>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ProfileServiceConnector _profileConnector;

        public UpdateCustomerLogic(
            IUserRepository userRepository,
            ProfileServiceConnector profileConnector)
        {
            _userRepository = userRepository;
            _profileConnector = profileConnector;
        }

        public async Task<UpdateCustomerResultData> Execute(UpdateCustomerParam param)
        {
            if (param == null)
                throw new ValidationException("Param cannot be null.");

            var user = await _userRepository.GetUserById(param.TargetUserId);
            if (user == null || user.IsDeleted)
                throw new NotFoundException("User not found.");

            if (user.Role != UserRoleEnum.Customer)
                throw new NotFoundException("Customer not found.");

            if (param.Points != null)
            {
                var callerRole = (param.CallerRole ?? string.Empty).ToLowerInvariant();
                var callerPosition = (param.CallerPosition ?? string.Empty).ToLowerInvariant();

                if (callerRole == UserRoleEnum.Admin)
                {
                }
                else
                {
                    if (callerRole != UserRoleEnum.Staff ||
                        callerPosition != StaffPositionEnum.OperationsManager)
                    {
                        throw new UnauthorizedException("You do not have permission to update this customer.");
                    }
                }
            }

            var profileResult = await _profileConnector.UpdateCustomerByUserId(
                userId: user.Id,
                fullName: param.FullName,
                phoneNumber: param.PhoneNumber,
                dayOfBirth: param.DayOfBirth,
                gender: param.Gender,
                points: param.Points
            );

            return new UpdateCustomerResultData
            {
                Result = profileResult.Result,
                Message = profileResult.Message,
                StatusCode = StatusCodeEnum.Success
            };
        }
    }
}
