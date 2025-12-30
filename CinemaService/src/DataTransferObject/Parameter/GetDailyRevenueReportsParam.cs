using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class GetDailyRevenueReportsParam : IParam
    {
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public Guid? CinemaId { get; set; }
    }
}
