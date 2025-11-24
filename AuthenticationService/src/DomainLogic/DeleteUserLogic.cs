using AuthenticationService.DataTransferObject.Parameter;
using AuthenticationService.DataTransferObject.ResultData;
using AuthenticationService.Infrastructure.Repositories.Interfaces;
using AuthenticationService.ServiceConnector.ProfileService;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace AuthenticationService.DomainLogic
{
    public class DeleteUserLogic : IDomainLogic<DeleteUserParam, Task<DeleteUserResultData>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ProfileServiceConnector _profileServiceConnector;

        public DeleteUserLogic(
            IUserRepository userRepository,
            ProfileServiceConnector profileServiceConnector)
        {
            _userRepository = userRepository;
            _profileServiceConnector = profileServiceConnector;
        }

        public async Task<DeleteUserResultData> Execute(DeleteUserParam param)
        {
            if (param == null)
                throw new ValidationException("Param cannot be null.");

            var targetUser = await _userRepository.GetUserById(param.TargetUserId);

            if (targetUser == null || targetUser.IsDeleted)
                throw new NotFoundException("User not found.");

            // 1. Không cho xoá Admin
            if (targetUser.Role == UserRoleEnum.Admin)
            {
                throw new UnauthorizedException("Admin user cannot be deleted.");
            }



            string? targetStaffPosition = null;


            if (targetUser.Role == UserRoleEnum.Staff)
            {
                var staffProfile = await _profileServiceConnector.GetStaffByUserId(targetUser.Id);
                targetStaffPosition = staffProfile?.Position;
            }

            var callerRole = param.CallerRole?.ToLowerInvariant() ?? string.Empty;
            var callerPosition = param.CallerPosition?.ToLowerInvariant();

            if (!IsDeletionAllowed(
                                    targetUser.Role,
                                    targetStaffPosition,
                                    callerRole,
                                    callerPosition))
            {
                throw new UnauthorizedException("You do not have permission to delete this user.");
            }


            await _userRepository.DeleteUser(targetUser);
            
            try
            {
                if (targetUser.Role == UserRoleEnum.Customer)
                {
                    await _profileServiceConnector.DeleteCustomerByUserId(targetUser.Id);
                }
                else if (targetUser.Role == UserRoleEnum.Staff)
                {
                    await _profileServiceConnector.DeleteStaffByUserId(targetUser.Id);
                }
            }
            catch
            {
            }

            return new DeleteUserResultData
            {
                Result = true,
                Message = "Delete user successfully.",
                StatusCode = StatusCodeEnum.Success
            };
        }

       private bool IsDeletionAllowed(
            string targetRole,
            string? targetStaffPosition,
            string callerRole,
            string? callerPosition)
        {
            targetRole = targetRole.ToLowerInvariant();
            callerRole = callerRole.ToLowerInvariant();
            callerPosition = callerPosition?.ToLowerInvariant();

            if (callerRole == UserRoleEnum.Admin)
            {
                if (targetRole == UserRoleEnum.Admin)
                    return false;

                return true;
            }

            if (callerRole != UserRoleEnum.Staff)
                return false;

            if (string.IsNullOrWhiteSpace(callerPosition))
                return false;

            var isCinemaManager     = callerPosition == StaffPositionEnum.CinemaManager;
            var isOperationsManager = callerPosition == StaffPositionEnum.OperationsManager;

            if (targetRole == UserRoleEnum.Admin)
                return false;

            if (targetRole == UserRoleEnum.Customer)
            {
                return isOperationsManager;
            }

            if (targetRole == UserRoleEnum.Staff)
            {
                if (string.IsNullOrWhiteSpace(targetStaffPosition))
                    return false;

                var callerLevel = GetPositionLevel(callerPosition);
                var targetLevel = GetPositionLevel(targetStaffPosition);

                if (callerLevel <= targetLevel)
                    return false;

                return true;
            }

            return false;
        }

        private int GetPositionLevel(string? position)
        {
            if (string.IsNullOrWhiteSpace(position))
                return 0;

            position = position.ToLowerInvariant();

            return position switch
            {
                StaffPositionEnum.Staff => 1,
                StaffPositionEnum.CinemaManager => 2,
                StaffPositionEnum.OperationsManager => 3,
                _ => 0
            };
        }
    }
}
