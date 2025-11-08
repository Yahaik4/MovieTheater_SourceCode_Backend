using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Grpc.Core;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using System.Text.Json;

namespace CinemaService.DomainLogic
{
    public class UpdateSeatLogic : IDomainLogic<UpdateSeatParam, Task<UpdateSeatsResultData>>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly ISeatTypeRepository _seatTypeRepository;
        public UpdateSeatLogic(ISeatRepository seatRepository, ISeatTypeRepository seatTypeRepository)
        {
            _seatRepository = seatRepository;
            _seatTypeRepository = seatTypeRepository;
        }

        public async Task<UpdateSeatsResultData> Execute(UpdateSeatParam param)
        {
            Console.WriteLine("Param: " + JsonSerializer.Serialize(param));

            if (param == null || param.Id == null || !param.Id.Any())
            {
                throw new ValidationException("Invalid or missing parameter Id");
            }

            if (param.SeatTypeId.HasValue) { 
                var seatType = await _seatTypeRepository.GetSeatTypeById(param.SeatTypeId.Value);
            
                if (seatType == null)
                {
                    throw new NotFoundException("Type Not Found");
                }
            }


            var seats = await _seatRepository.GetSeatByIds(param.Id);

            foreach(var seat in seats){

                if (param.isActive.HasValue)
                {
                    seat.isActive = param.isActive.Value;
                }

                if (param.SeatTypeId.HasValue)
                {
                    seat.SeatTypeId = param.SeatTypeId.Value;
                }
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
