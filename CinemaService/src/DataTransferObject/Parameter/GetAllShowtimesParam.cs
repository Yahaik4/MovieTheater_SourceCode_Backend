using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class GetAllShowtimesParam : IParam
    {
        public Guid? CinemaId { get; set; }
        public Guid? MovieId { get; set; }
        public DateOnly? Date { get; set; }
        public string? Country { get; set; }
    }
}
