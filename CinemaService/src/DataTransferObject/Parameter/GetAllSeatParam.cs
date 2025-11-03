using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class GetAllSeatParam : IParam
    {
        public Guid RoomId { get; set; }
    }
}
