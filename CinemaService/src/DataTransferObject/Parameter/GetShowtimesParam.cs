using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class GetShowtimesParam : IParam
    {
        public Guid? Id { get; set; }
        public Guid MovieId { get; set; }
        public string Country { get; set; }
        public DateOnly Date {  get; set; }
    }
}
