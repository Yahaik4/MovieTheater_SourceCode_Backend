using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class CreateHolidayParam : IParam
    {
        public string Name { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public decimal ExtraPrice { get; set; }
    }
}
