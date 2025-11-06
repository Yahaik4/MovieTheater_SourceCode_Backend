using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class DeletePersonParam : IParam
    {
        public Guid Id { get; set; }
    }
}
