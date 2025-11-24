using AuthenticationService.DataTransferObject.Parameter;
using AuthenticationService.DataTransferObject.ResultData;
using AuthenticationService.Infrastructure.Repositories.Interfaces;
using AuthenticationService.ServiceConnector.ProfileService;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace AuthenticationService.DomainLogic
{
    public class UpdateStaffLogic : IDomainLogic<UpdateStaffParam, Task<UpdateStaffResultData>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ProfileServiceConnector _profileConnector;

        public UpdateStaffLogic(
            IUserRepository userRepository,
            ProfileServiceConnector profileConnector)
        {
            _userRepository = userRepository;
            _profileConnector = profileConnector;
        }

        public async Task<UpdateStaffResultData> Execute(UpdateStaffParam param)
        {
            if (param == null)
                throw new ValidationException("Param cannot be null.");

            var user = await _userRepository.GetUserById(param.TargetUserId);
            if (user == null || user.IsDeleted)
                throw new NotFoundException("User not found.");

            if (user.Role != UserRoleEnum.Staff)
                throw new NotFoundException("Staff not found.");

            var staffProfile = await _profileConnector.GetStaffByUserId(user.Id);
            if (staffProfile == null)
                throw new NotFoundException("Staff profile not found.");

            var targetPosition = staffProfile.Position;

            var callerRole = (param.CallerRole ?? string.Empty).ToLowerInvariant();
            var callerPosition = (param.CallerPosition ?? string.Empty).ToLowerInvariant();

            if (!IsUpdateAllowed(targetPosition, callerRole, callerPosition))
            {
                throw new UnauthorizedException("You do not have permission to update this staff.");
            }

            var profileResult = await _profileConnector.UpdateStaffByUserId(
                userId: user.Id,
                fullName: param.FullName,
                phoneNumber: param.PhoneNumber,
                dayOfBirth: param.DayOfBirth,
                gender: param.Gender,
                cinemaId: param.CinemaId,
                position: param.Position,
                salary: param.Salary
            );

            return new UpdateStaffResultData
            {
                Result = profileResult.Result,
                Message = profileResult.Message,
                StatusCode = StatusCodeEnum.Success
            };
        }

        private bool IsUpdateAllowed(string targetStaffPosition, string callerRole, string callerPosition)
        {
            callerRole = callerRole.ToLowerInvariant();
            callerPosition = callerPosition.ToLowerInvariant();
            var targetPos = targetStaffPosition.ToLowerInvariant();

            if (callerRole == UserRoleEnum.Admin)
                return true;

            if (callerRole != UserRoleEnum.Staff)
                return false;

            var isCallerCinema = callerPosition == StaffPositionEnum.CinemaManager;
            var isCallerOps = callerPosition == StaffPositionEnum.OperationsManager;

            var isTargetStaff = targetPos == StaffPositionEnum.Staff;
            var isTargetCinema = targetPos == StaffPositionEnum.CinemaManager;
            var isTargetOps = targetPos == StaffPositionEnum.OperationsManager;

            if (isTargetOps)
                return false;

            if (isTargetStaff)
                return isCallerCinema || isCallerOps;

            if (isTargetCinema)
                return isCallerOps;

            return false;
        }
    }
}
