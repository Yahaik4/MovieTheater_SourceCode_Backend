using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class UpdateSeatParam : IParam
    {
        public List<Guid> Id { get; set; }
        public bool? isActive { get; set; }
        public Guid? SeatTypeId { get; set; }
    }
}
