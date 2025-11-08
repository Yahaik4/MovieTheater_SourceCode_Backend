using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class GetAllRoomTypeParam : IParam
    {
        public Guid? Id { get; set; }
        public string? Type { get; set; }
        public decimal? BasePrice { get; set; }
    }
}
