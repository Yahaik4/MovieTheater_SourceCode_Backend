using CinemaService.DataTransferObject.Parameter;
using CinemaService.DataTransferObject.ResultData;
using CinemaService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CinemaService.DomainLogic
{
    public class DeleteSeatTypeLogic : IDomainLogic<DeleteSeatTypeParam, Task<DeleteSeatTypeResultData>>
    {
        private readonly ISeatTypeRepository _seatTypeRepository;
        public DeleteSeatTypeLogic(ISeatTypeRepository seatTypeRepository)
        {
            _seatTypeRepository = seatTypeRepository;
        }

        public async Task<DeleteSeatTypeResultData> Execute(DeleteSeatTypeParam param)
        {
            var seatType = await _seatTypeRepository.GetSeatTypeById(param.Id);

            if (seatType == null)
            {
                throw new NotFoundException("Type Not Found");
            }

            seatType.IsDeleted = true;

            await _seatTypeRepository.UpdateSeatType(seatType);

            return new DeleteSeatTypeResultData
            {
                Result = true,
                StatusCode = StatusCodeEnum.Success,
                Message = "Delete Successfully",
            };
        }
    }
}
