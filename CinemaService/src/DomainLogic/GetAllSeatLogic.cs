using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
{
    public class GetAllSeatLogic : IDomainLogic<GetAllSeatParam, Task<GetAllSeatResultData>>
    {
        private readonly ISeatRepository _seatRepository;

        public GetAllSeatLogic(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public async Task<GetAllSeatResultData> Execute(GetAllSeatParam param)
        {
            var seats = await _seatRepository.GetAllSeatByRoom(param.RoomId);

            return new GetAllSeatResultData
            {
                Result = true,
                Message = "Get all seat Successfully",
                StatusCode = StatusCodeEnum.Success,
                Data = seats.Select(s => new GetAllSeatDataResult
                {
                    Id = s.Id,
                    Label = s.Label,
                    ColumnIndex = s.ColumnIndex,
                    DisplayNumber = s.DisplayNumber,
                    SeatCode = s.SeatCode,
                    isActive = s.isActive,
                    Status = s.Status,
                    SeatType = s.SeatType.Type,
                }).ToList()
            };
        }
    }
}
