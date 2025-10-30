using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class DeleteSeatTypeParam : IParam
    {
        public Guid Id { get; set; }
    }
}
