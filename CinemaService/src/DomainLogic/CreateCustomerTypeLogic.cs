using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class CreateCustomerTypeLogic : IDomainLogic<CreateCustomerTypeParam, Task<CreateCustomerTypeResultData>>
    {
        private readonly ICustomerTypeRepository _customerTypeRepository;

        public CreateCustomerTypeLogic(ICustomerTypeRepository customerTypeRepository)
        {
            _customerTypeRepository = customerTypeRepository;
        }

        public async Task<CreateCustomerTypeResultData> Execute(CreateCustomerTypeParam param)
        {
            var customerType = await _customerTypeRepository.GetCustomerTypeByName(param.Name);

            if (customerType != null) 
            {
                throw new ConflictException("Customer Type is existed");
            }

            var newCustomerType = new CustomerType
            {
                Id = Guid.NewGuid(),
                Name = param.Name,
                RoleCondition = param.RoleCondition,
            };

            await _customerTypeRepository.CreateCustomerType(newCustomerType);

            return new CreateCustomerTypeResultData
            {
                Result = true,
                Message = "Create Customer Type Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new CreateCustomerTypeDataResult
                {
                    Id = newCustomerType.Id,
                    Name = newCustomerType.Name,
                    RoleCondition = newCustomerType.RoleCondition,
                }
            };
        }
    }
}
