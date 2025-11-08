using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class DeleteRoomParam : IParam
    {
        public Guid Id { get; set; }
    }
}
