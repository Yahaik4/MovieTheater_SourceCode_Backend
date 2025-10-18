using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Helper;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace src.DomainLogic
{
    public class CreateCinemaLogic : IDomainLogic<CreateCinemaParam, Task<CreateCinemaResultData>>
    {
        private readonly ICinemaRepository _cinemaRepository;

        public CreateCinemaLogic(ICinemaRepository cinemaRepository)
        {
            _cinemaRepository = cinemaRepository;
        }

        public async Task<CreateCinemaResultData> Execute(CreateCinemaParam param)
        {
            if (param == null)
            {
                throw new ValidationException("Param cannot be blank.");
            }

            var cinema = new Cinema
            {
                Id = Guid.NewGuid(),
                Name = param.Name,
                Address = param.Address,
                PhoneNumber = param.PhoneNumber,
                Email = param.Email,
                Open_Time = param.Open_Time,
                Close_Time = param.Close_Time,
                Status = string.IsNullOrWhiteSpace(param.Status) ? "Active" : param.Status,
            };

            await _cinemaRepository.CreateCinema(cinema);

            return new CreateCinemaResultData
            {
                Result = true,
                Message = "Create new Cinema Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = new CreateCinemaDataResult
                {
                    Id = cinema.Id,
                    Name = cinema.Name,
                    Address = cinema.Address,
                    PhoneNumber = cinema.PhoneNumber,
                    Email = cinema.Email,
                    Open_Time = cinema.Open_Time,
                    Close_Time = cinema.Close_Time,
                    Status = cinema.Status,
                    TotalRoom = cinema.TotalRoom
                }
            };
        } 
    }
}
