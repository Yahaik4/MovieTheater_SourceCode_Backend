using Shared.Contracts.ResultData;

namespace CinemaService.DataTransferObject.ResultData
{
    public class UpdateHolidayResultData : BaseResultData
    { 
        public UpdateHolidayDataResult Data { get; set; }
    }

    public class UpdateHolidayDataResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public decimal ExtraPrice { get; set; }
    }
}
