using Shared.Contracts.ResultData;
using System;

namespace CinemaService.DataTransferObject.ResultData
{
    public class GetHolidaysResultData : BaseResultData
    {
        public List<GetHolidaysDataResult> Data { get; set; }
    }

    public class GetHolidaysDataResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public decimal ExtraPrice { get; set; }
    }
}
