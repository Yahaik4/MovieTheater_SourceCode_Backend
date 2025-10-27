﻿using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
{
    public class DeleteCinemaLogic : IDomainLogic<DeleteCinemaParam, Task<DeleteCinemaResultData>>
    {
        private readonly ICinemaRepository _cinemaRepository;
        public DeleteCinemaLogic(ICinemaRepository cinemaRepository)
        {
            _cinemaRepository = cinemaRepository;
        }

        public async Task<DeleteCinemaResultData> Execute(DeleteCinemaParam param)
        {
            var cinema = await _cinemaRepository.GetCinemaById(param.Id);

            if (cinema == null) {
                throw new NotFoundException("Cinema Not Found");
            }

            cinema.IsDeleted = true;

            await _cinemaRepository.UpdateCinema(cinema);

            return new DeleteCinemaResultData
            {
                Result = true,
                StatusCode = StatusCodeEnum.Success,
                Message = "Delete Cinema Successfully"
            };
        }
    }
}
