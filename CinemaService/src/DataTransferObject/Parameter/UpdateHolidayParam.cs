using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class UpdateHolidayParam : IParam
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } // Valentine’s day
        public int? Day { get; set; }
        public int? Month { get; set; }
        public decimal? ExtraPrice { get; set; } // +10k
    }
}
