using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
{
    public class CreateSeatTypeLogic : IDomainLogic<CreateSeatTypeParam, Task<CreateSeatTypeResultData>>
    {
        private readonly ISeatTypeRepository _seatTypeRepository;

        public CreateSeatTypeLogic(ISeatTypeRepository seatTypeRepository)
        {
            _seatTypeRepository = seatTypeRepository;
        }

        public async Task<CreateSeatTypeResultData> Execute(CreateSeatTypeParam param)
        {
            var existed = await _seatTypeRepository.GetSeatTypeByName(param.Type);

            if (existed != null) {
                throw new ValidationException("Type Conflict");
            }

            var newType = new SeatType
            {
                Id = Guid.NewGuid(),
                Type = param.Type,
                ExtraPrice = param.ExtraPrice,
            };

            await _seatTypeRepository.CreateSeatType(newType);

            return new CreateSeatTypeResultData
            {
                Result = true,
                StatusCode = StatusCodeEnum.Created,
                Message = "Create Seat Type Successfully",
                Data = new CreateSeatTypeDataResult
                {
                    Id = newType.Id,
                    Type = newType.Type,
                    ExtraPrice = newType.ExtraPrice,
                }
            };
        }
    }
}
