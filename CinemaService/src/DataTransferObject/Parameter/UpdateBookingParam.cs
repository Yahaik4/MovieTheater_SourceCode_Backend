using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class UpdateBookingParam : IParam
    {
        public Guid BookingId { get; set; }
        public string Status { get; set; } //Pending | Paid | Cancelled | Expired | Failed
    }
}
