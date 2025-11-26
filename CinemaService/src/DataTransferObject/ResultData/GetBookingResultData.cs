using CinemaService.Infrastructure.EF.Models;
using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class GetBookingResultData : BaseResultData
    {
        public GetBookingDataResult Data { get; set; }
    }

    public class GetBookingDataResult
    {
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } //Pending | Paid | Cancelled | Expired | Failed
        public decimal TotalPrice { get; set; }
    }
}
