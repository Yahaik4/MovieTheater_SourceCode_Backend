using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class GetShowtimeDetailsParam : IParam
    {
        public Guid ShowtimeId { get; set; }
    }
}
