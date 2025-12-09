using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class GetShowtimesByCinemaParam : IParam
    {
        public Guid CinemaId { get; set; }
        public DateOnly Date { get; set; }
    }
}
