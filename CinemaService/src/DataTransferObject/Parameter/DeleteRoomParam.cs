using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class DeleteRoomParam : IParam
    {
        public Guid Id { get; set; }
    }
}
