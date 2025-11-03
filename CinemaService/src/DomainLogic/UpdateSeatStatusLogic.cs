using Grpc.Core;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
{
    public class UpdateSeatStatusLogic : IDomainLogic<UpdateSeatStatusParam, Task<UpdateSeatsResultData>>
    {
        private readonly ISeatRepository _seatRepository;

        public UpdateSeatStatusLogic(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public async Task<UpdateSeatsResultData> Execute(UpdateSeatStatusParam param)
        {
            if (param == null || param.Id == null || !param.Id.Any() || param.Status == null)
            {
                throw new ValidationException("Invalid or missing parameter Id");
            }

            var seats = await _seatRepository.GetSeatByIds(param.Id);

            foreach (var seat in seats)
            {
                seat.Status = param.Status;   
            }

            await _seatRepository.UpdateSeats(seats);

            return new UpdateSeatsResultData
            {
                Result = true,
                StatusCode = StatusCodeEnum.Success,
                Message = "Update seats Successfully",
                Data = seats.Select(s => new UpdateSeatsDataResult
                {
                    Label = s.Label,
                    ColumnIndex = s.ColumnIndex,
                    DisplayNumber = s.DisplayNumber,
                    SeatCode = s.SeatCode,
                    isActive = s.isActive,
                    Status = s.Status,
                    SeatType = s.SeatType.Type
                }).ToList()
            };
        }
    }
}
