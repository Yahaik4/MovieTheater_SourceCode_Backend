using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Exceptions;
using src.Infrastructure.EF.Models;
using Shared.Contracts.Enums;

namespace src.DomainLogic
{
    public class CreateProfileLogic : IDomainLogic<CreateProfileParam, Task<CreateProfileResultData>>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateProfileLogic(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CreateProfileResultData> Execute(CreateProfileParam param)
        {
            if (param == null)
            {
                throw new ValidationException("Param cannot be blank.");
            }

            if(param.Role == "customer")
            {
                var profile = await _customerRepository.CreateCustomer(new Customer
                {
                    FullName = param.FullName,
                    UserId = param.UserId,
                });
            }
            else
            {

            }


            return new CreateProfileResultData
            {
                Result = true,
                Message = "Create profile Successfully",
                StatusCode = StatusCodeEnum.Success,
            };

        }
    }
}
