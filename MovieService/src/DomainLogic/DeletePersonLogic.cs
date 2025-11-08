using MovieService.DataTransferObject.Parameter;
using MovieService.DataTransferObject.ResultData;
using MovieService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace MovieService.DomainLogic
{
    public class DeletePersonLogic : IDomainLogic<DeletePersonParam, Task<DeletePersonResultData>>
    {
        private readonly IPersonRepository _personRepository;

        public DeletePersonLogic(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<DeletePersonResultData> Execute(DeletePersonParam param)
        {

            var person = await _personRepository.GetPersonById(param.Id);

            if (person == null) {
                throw new ValidationException($"Person don't already exists.");
            }

            person.IsDeleted = true;

            await _personRepository.UpdatePerson(person);

            return new DeletePersonResultData
            {
                Result = true,
                Message = "Delete Person Successfully",
                StatusCode = StatusCodeEnum.Success,
            };
        }
    }
}
