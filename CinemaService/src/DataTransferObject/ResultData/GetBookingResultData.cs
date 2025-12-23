using CinemaService.Infrastructure.EF.Models;
using Grpc.Net.Client.Balancer;
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
        public Guid CinemaId { get; set; }
        public string Cinema { get; set; }
        public string Address { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public decimal TotalPrice { get; set; }
        public List<GetSeatsBooking> Seats { get; set; }
    }

    public class GetSeatsBooking
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string SeatType { get; set; }
    }
}
