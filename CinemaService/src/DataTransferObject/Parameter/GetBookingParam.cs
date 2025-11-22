using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class GetBookingParam : IParam
    {
        public Guid BookingId { get; set; }
    }
}
