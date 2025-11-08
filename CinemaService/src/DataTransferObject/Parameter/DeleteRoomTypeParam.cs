using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class DeleteRoomTypeParam : IParam
    {
        public Guid Id { get; set; }
    }
}
