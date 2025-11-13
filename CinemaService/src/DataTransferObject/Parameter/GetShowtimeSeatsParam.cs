using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class GetShowtimeSeatsParam : IParam
    {
        public Guid showtimeId { get; set; }
    }
}
