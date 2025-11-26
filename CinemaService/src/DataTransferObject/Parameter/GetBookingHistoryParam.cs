using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class GetBookingHistoryParam : IParam
    {
        public Guid UserId { get; set; }
    }
}
