using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using CinemaService.Helper;
using CinemaService.Infrastructure.EF.Models;

namespace CinemaService.DomainLogic
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
                City = param.City,
                PhoneNumber = param.PhoneNumber,
                Email = param.Email,
                Open_Time = param.OpenTime,
                Close_Time = param.CloseTime,
                Status = param.Status,
                CreatedBy = param.CreateBy
            };

            await _cinemaRepository.CreateCinema(cinema);

            return new CreateCinemaResultData
            {
                Result = true,
                Message = "Create new Cinema Successfully",
                StatusCode = StatusCodeEnum.Created,
                Data = new CreateCinemaDataResult
                {
                    Id = cinema.Id,
                    Name = cinema.Name,
                    Address = cinema.Address,
                    City = cinema.City,
                    PhoneNumber = cinema.PhoneNumber,
                    Email = cinema.Email,
                    OpenTime = cinema.Open_Time,
                    CloseTime = cinema.Close_Time,
                    Status = cinema.Status,
                    CreateBy = cinema.CreatedBy
                }
            };
        } 
    }
}
