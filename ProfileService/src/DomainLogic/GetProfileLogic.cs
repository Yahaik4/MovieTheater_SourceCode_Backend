using ProfileService.DataTransferObject.Parameter;
using ProfileService.DataTransferObject.ResultData;
using ProfileService.Infrastructure.EF.Models;
using ProfileService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace ProfileService.DomainLogic
{
    public class GetProfileLogic : IDomainLogic<GetProfileParam, Task<GetProfileResultData>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetProfileLogic(
            ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<GetProfileResultData> Execute(GetProfileParam param)
        {
            if (param == null)
            {
                throw new ValidationException("Param cannot be null.");
            }

            var customer = await _customerRepository.GetCustomerByUserId(param.UserId);

            if (customer == null) {
                throw new NotFoundException("Customer not found");
            }

            return new GetProfileResultData
            {
                Result = true,
                Message = "Get profile Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new GetProfileDataResult
                {
                    FullName = customer.FullName,
                    Address = customer.Address,
                    DayOfBirth = customer.DayOfBirth,
                    Gender = customer.Gender,
                    PhoneNumer = customer.PhoneNumer,
                    Points = customer.Points
                }
            };
        }
    }
}
