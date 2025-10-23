using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
{
    public class UpdateCinemaLogic : IDomainLogic<UpdateCinemaParam, Task<UpdateCinemaDataResult>>
    {
        private readonly ICinemaRepository _cinemaRepository;
        public UpdateCinemaLogic(ICinemaRepository cinemaRepository) 
        {
            _cinemaRepository = cinemaRepository;
        }

        public Task<UpdateCinemaDataResult> Execute(UpdateCinemaParam param)
        {
            throw new NotImplementedException();
        }
    }
}
