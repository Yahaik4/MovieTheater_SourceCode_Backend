using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class UpdateCinemaLogic : IDomainLogic<UpdateCinemaParam, Task<UpdateCinemaResultData>>
    {
        private readonly ICinemaRepository _cinemaRepository;
        public UpdateCinemaLogic(ICinemaRepository cinemaRepository) 
        {
            _cinemaRepository = cinemaRepository;
        }

        public async Task<UpdateCinemaResultData> Execute(UpdateCinemaParam param)
        {
            var cinema = await _cinemaRepository.GetCinemaById(param.Id);

            if (cinema == null) {
                throw new NotFoundException("Cinema Not Found");
            }

            cinema.Name = param.Name;
            cinema.PhoneNumber = param.PhoneNumber;
            cinema.Address = param.Address;
            cinema.City = param.City;
            cinema.Email = param.Email;
            cinema.Open_Time = param.OpenTime;
            cinema.Close_Time = param.CloseTime;
            cinema.Status = param.Status;
            cinema.UpdatedAt = DateTime.UtcNow;

            await _cinemaRepository.UpdateCinema(cinema);

            return new UpdateCinemaResultData
            {
                Result = true,
                StatusCode = StatusCodeEnum.Success,
                Message = "Update Cinema Successfully",
                Data = new UpdateCinemaDataResult
                {
                    Name = param.Name,
                    PhoneNumber = param.PhoneNumber,
                    Address = param.Address,
                    City = param.City,
                    Email = param.Email,
                    Status = param.Status,
                    OpenTime = param.OpenTime,
                    CloseTime = param.CloseTime,
                }
            };
        }
    }
}
