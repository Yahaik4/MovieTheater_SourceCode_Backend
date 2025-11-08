using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class GetAllSeatParam : IParam
    {
        public Guid RoomId { get; set; }
    }
}
