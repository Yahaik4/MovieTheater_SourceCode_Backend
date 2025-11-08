using MovieService.DataTransferObject.Parameter;
using MovieService.DataTransferObject.ResultData;
using MovieService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace MovieService.DomainLogic
{
    public class UpdatePersonLogic : IDomainLogic<UpdatePersonParam, Task<UpdatePersonResultData>>
    {
        private readonly IPersonRepository _personRepository;

        public UpdatePersonLogic(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<UpdatePersonResultData> Execute(UpdatePersonParam param)
        {

            var person = await _personRepository.GetPersonById(param.Id);

            if (person == null) 
            {
                throw new NotFoundException($"Person {param.FullName} don't exists.");
            }

            person.FullName = param.FullName;
            person.Gender = param.Gender;
            person.BirthDate = param.BirthDate;
            person.Bio = param.Bio;
            person.ImageUrl = param.ImageUrl;
            person.Nationality = param.Nationality;
            person.UpdatedAt = DateTime.UtcNow;

            await _personRepository.UpdatePerson(person);

            return new UpdatePersonResultData
            {
                Result = true,
                Message = "Create Person Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new UpdatePersonDataResult
                {
                    Id = person.Id,
                    FullName = person.FullName,
                    Gender = person.Gender,
                    BirthDate = person.BirthDate,
                    Nationality = person.Nationality,
                    Bio = person.Bio,
                    ImageUrl = person.ImageUrl,
                }
            };
        }
    }
}
