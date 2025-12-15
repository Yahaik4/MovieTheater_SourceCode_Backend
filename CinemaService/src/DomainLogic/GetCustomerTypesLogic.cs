using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class GetCustomerTypesLogic : IDomainLogic<GetCustomerTypesParam, Task<GetCustomerTypesResultData>>
    {
        private readonly ICustomerTypeRepository _customerTypeRepository;

        public GetCustomerTypesLogic(ICustomerTypeRepository customerTypeRepository)
        {
            _customerTypeRepository = customerTypeRepository;
        }

        public async Task<GetCustomerTypesResultData> Execute(GetCustomerTypesParam param)
        {
            var customerTypes = await _customerTypeRepository.GetCustomerTypes(param.Id, param.Name, param.RoleCondition);

            return new GetCustomerTypesResultData
            {
                Result = true,
                Message = "Get Customer Types Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = customerTypes.Select(c => new GetCustomerTypesDataResult
                {
                    Id = c.Id,
                    Name = c.Name,
                    RoleCondition = c.RoleCondition,
                }).ToList()
            };
        }
    }
}
