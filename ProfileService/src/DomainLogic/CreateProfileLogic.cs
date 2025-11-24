using ProfileService.DataTransferObject.Parameter;
using ProfileService.DataTransferObject.ResultData;
using ProfileService.Infrastructure.EF.Models;
using ProfileService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace ProfileService.DomainLogic
{
    public class CreateProfileLogic : IDomainLogic<CreateProfileParam, Task<CreateProfileResultData>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IStaffRepository _staffRepository;

        public CreateProfileLogic(
            ICustomerRepository customerRepository,
            IStaffRepository staffRepository)
        {
            _customerRepository = customerRepository;
            _staffRepository = staffRepository;
        }

        public async Task<CreateProfileResultData> Execute(CreateProfileParam param)
        {
            if (param == null)
            {
                throw new ValidationException("Param cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(param.Role))
            {
                throw new ValidationException("Role is required.");
            }

            if (param.Role == UserRoleEnum.Customer)
            {
                await _customerRepository.CreateCustomer(new Customer
                {
                    UserId = param.UserId,
                    FullName = param.FullName
                });
            }
            else if (param.Role == UserRoleEnum.Staff)
            {
                if (!param.CinemaId.HasValue)
                {
                    throw new ValidationException("CinemaId is required for staff.");
                }

                if (string.IsNullOrWhiteSpace(param.Position))
                {
                    throw new ValidationException("Position is required for staff.");
                }

                await _staffRepository.CreateStaff(new Staff
                {
                    UserId = param.UserId,
                    FullName = param.FullName,
                    Salary = param.Salary ?? 0,
                    PhoneNumber = param.PhoneNumber,
                    DayOfBirth = param.DayOfBirth,
                    Gender = param.Gender,
                    CinemaId = param.CinemaId.Value,
                    Position = param.Position,
                });
            }
            else
            {
                throw new ValidationException($"Unsupported role: {param.Role}");
            }

            return new CreateProfileResultData
            {
                Result = true,
                Message = "Create profile successfully",
                StatusCode = StatusCodeEnum.Success
            };
        }
    }
}
