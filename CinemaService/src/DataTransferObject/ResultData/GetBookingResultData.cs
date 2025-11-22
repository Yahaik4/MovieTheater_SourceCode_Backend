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
        public Guid UserId { get; set; }
        public string Status { get; set; } //Pending | Paid | Cancelled | Expired | Failed
        public decimal TotalPrice { get; set; }
    }
}
