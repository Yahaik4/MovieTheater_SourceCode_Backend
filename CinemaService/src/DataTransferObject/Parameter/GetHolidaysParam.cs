using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class GetHolidaysParam : IParam
    {
        public string? Name { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
