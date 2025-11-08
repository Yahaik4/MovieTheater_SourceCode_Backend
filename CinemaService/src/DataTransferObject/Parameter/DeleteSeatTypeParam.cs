using Shared.Contracts.Interfaces;

namespace CinemaService.DataTransferObject.Parameter
{
    public class DeleteSeatTypeParam : IParam
    {
        public Guid Id { get; set; }
    }
}
