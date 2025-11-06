using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
{
    public class GetPersonsLogic : IDomainLogic<GetPersonsParam, Task<GetPersonsResultData>>
    {
        private readonly IPersonRepository _personRepository;

        public GetPersonsLogic(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<GetPersonsResultData> Execute(GetPersonsParam param)
        {

            var persons = await _personRepository.GetPersons(param);

            return new GetPersonsResultData
            {
                Result = true,
                Message = "Get Persons Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = persons.Select(p => new GetPersonsDataResult
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    Gender = p.Gender,
                    BirthDate = p.BirthDate,
                    Nationality = p.Nationality,
                    Bio = p.Bio,
                    ImageUrl = p.ImageUrl,
                }).ToList()
            };
        }
    }
}
