using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class GetAllRoomParam : IParam
    {
        public Guid CinemaId { get; set; }
        public Guid? Id { get; set; }
        public int? RoomNumber { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
    }
}
