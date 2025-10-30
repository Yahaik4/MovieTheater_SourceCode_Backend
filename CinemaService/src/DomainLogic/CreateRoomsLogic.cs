using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;
using src.DataTransferObject.Parameter;
using src.DataTransferObject.ResultData;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;

namespace src.DomainLogic
{
    public class CreateRoomsLogic : IDomainLogic<CreateRoomParam, Task<CreateRoomResultData>>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;

        public CreateRoomsLogic(IRoomRepository roomRepository, ICinemaRepository cinemaRepository, IRoomTypeRepository roomTypeRepository)
        {
            _roomRepository = roomRepository;
            _cinemaRepository = cinemaRepository;
            _roomTypeRepository = roomTypeRepository;
        }

        public Task<CreateRoomResultData> Execute(CreateRoomParam param)
        {
            throw new NotImplementedException();
        }

        //public async Task<CreateRoomResultData> Execute(CreateRoomParam param)
        //{
        //    var cinema = await _cinemaRepository.GetCinemaById(param.CinemaId);

        //    if (cinema == null) {
        //        throw new NotFoundException("Cinema not Found");
        //    }

        //    var type = await _roomTypeRepository.GetRoomTypeById(param.RoomTypeId);

        //    if (type == null) {
        //        throw new NotFoundException("Type not Found");
        //    }



        //}
    }
}
