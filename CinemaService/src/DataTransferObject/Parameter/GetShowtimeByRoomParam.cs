using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class GetShowtimeByRoomParam : IParam
    {
        public Guid RoomId { get; set; }
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
    }
}
