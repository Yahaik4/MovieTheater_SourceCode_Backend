using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class GetOverviewParam : IParam
    {
        public string? RangeType { get; set; } // day, week, month, year
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Guid? CinemaId { get; set; }
    }
}
