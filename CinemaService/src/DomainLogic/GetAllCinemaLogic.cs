using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.Repositories.Interfaces;
using System.Text.Json;

namespace src.DomainLogic
{
    public class GetAllCinemaLogic : IDomainLogic<GetAllCinemasParam, Task<GetAllCinemasResultData>>
    {
        private readonly ICinemaRepository _cinemaRepository;

        public GetAllCinemaLogic(ICinemaRepository cinemaRepository)
        {
            _cinemaRepository = cinemaRepository;
        }

        public async Task<GetAllCinemasResultData> Execute(GetAllCinemasParam param)
        {
            var cinemas = await _cinemaRepository.GetAllCinema(param.Id, param.Name, param.City, param.Status);

            return new GetAllCinemasResultData
            {
                Result = true,
                Message = "Get All Cinemas Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = cinemas.Select(c => new GetAllCinemasDataResult
                {
                    Id = c.Id,
                    Name = c.Name,
                    Address = c.Address,
                    City = c.City,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    OpenTime = c.Open_Time,
                    CloseTime = c.Close_Time,
                    TotalRoom = c.Rooms?.Count() ?? 0,
                    Status = c.Status,
                }).ToList(),
            };
        }
    }
}
