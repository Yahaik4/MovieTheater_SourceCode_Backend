using MovieService.DataTransferObject.Parameter;
using MovieService.DataTransferObject.ResultData;
using MovieService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using MovieService.Infrastructure.EF.Models;

namespace MovieService.DomainLogic
{
    public class CreatePersonLogic : IDomainLogic<CreatePersonParam, Task<CreatePersonResultData>>
    {
        private readonly IPersonRepository _personRepository;

        public CreatePersonLogic(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<CreatePersonResultData> Execute(CreatePersonParam param)
        {
            var newPerson = await _personRepository.CreatePerson(new Person
            {
                Id = Guid.NewGuid(),
                FullName = param.FullName,
                Gender = param.Gender,
                BirthDate = param.BirthDate,
                Nationality = param.Nationality,
                Bio = param.Bio,
                ImageUrl = param.ImageUrl,
                CreatedBy = param.CreatedBy,
                CreatedAt = DateTime.UtcNow
            });

            return new CreatePersonResultData
            {
                Result = true,
                Message = "Create Person Successfully",
                StatusCode = StatusCodeEnum.Created,
                Data = new CreatePersonDataResult
                {
                    Id = newPerson.Id,
                    FullName = newPerson.FullName,
                    Gender = newPerson.Gender,
                    BirthDate = newPerson.BirthDate,
                    Nationality = newPerson.Nationality,
                    Bio = newPerson.Bio,
                    ImageUrl = newPerson.ImageUrl,
                }
            };
        }
    }
}
