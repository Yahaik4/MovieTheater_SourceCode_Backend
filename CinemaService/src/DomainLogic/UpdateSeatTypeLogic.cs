﻿using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
{
    public class UpdateSeatTypeLogic : IDomainLogic<UpdateSeatTypeParam, Task<UpdateSeatTypeResultData>>
    {
        private readonly ISeatTypeRepository _seatTypeRepository;
        public UpdateSeatTypeLogic(ISeatTypeRepository seatTypeRepository)
        {
            _seatTypeRepository = seatTypeRepository;
        }

        public async Task<UpdateSeatTypeResultData> Execute(UpdateSeatTypeParam param)
        {
            var seatType = await _seatTypeRepository.GetSeatTypeById(param.Id);

            if (seatType == null)
            {
                throw new NotFoundException("Type Not Found");
            }

            seatType.Type = param.Type;
            seatType.ExtraPrice = param.ExtraPrice;
            seatType.UpdatedAt = DateTime.UtcNow;

            await _seatTypeRepository.UpdateSeatType(seatType);

            return new UpdateSeatTypeResultData
            {
                Result = true,
                StatusCode = StatusCodeEnum.Success,
                Message = "Update Cinema Successfully",
                Data = new UpdateSeatTypeDataResult
                {
                    Type = seatType.Type,
                    ExtraPrice = seatType.ExtraPrice,
                }
            };
        }
    }
}
