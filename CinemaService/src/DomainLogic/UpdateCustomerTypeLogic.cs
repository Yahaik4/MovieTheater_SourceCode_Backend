using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.EF.Models;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class UpdateCustomerTypeLogic : IDomainLogic<UpdateCustomerTypeParam, Task<UpdateCustomerTypeResultData>>
    {
        private readonly ICustomerTypeRepository _customerTypeRepository;

        public UpdateCustomerTypeLogic(ICustomerTypeRepository customerTypeRepository)
        {
            _customerTypeRepository = customerTypeRepository;
        }

        public async Task<UpdateCustomerTypeResultData> Execute(UpdateCustomerTypeParam param)
        {
            var customerType = await _customerTypeRepository.GetCustomerTypeById(param.Id);

            if (customerType == null)
            {
                throw new NotFoundException("Customer Type don't existed");
            }

            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                var check = await _customerTypeRepository.GetCustomerTypeByName(param.Name);

                if (check != null)
                {
                    throw new ConflictException("Customer Type is existed");
                }

                customerType.Name = param.Name;
            }

            if (!string.IsNullOrWhiteSpace(param.RoleCondition))
            {
                customerType.RoleCondition = param.RoleCondition;
            }

            await _customerTypeRepository.UpdateCustomerType(customerType);

            return new UpdateCustomerTypeResultData
            {
                Result = true,
                Message = "Update Customer Type Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new UpdateCustomerTypeDataResult
                {
                    Id = customerType.Id,
                    Name = customerType.Name,
                    RoleCondition = customerType.RoleCondition,
                }
            };
        }
    }
}
