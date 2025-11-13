using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class UpdateShowtimeParam : IParam
    {
        public Guid Id { get; set; }
        public Guid? MovieId { get; set; }
        public Guid? RoomId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Status { get; set; }
    }
}
