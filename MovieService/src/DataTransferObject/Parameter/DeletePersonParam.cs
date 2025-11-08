using Shared.Contracts.Interfaces;

namespace MovieService.DataTransferObject.Parameter
{
    public class DeletePersonParam : IParam
    {
        public Guid Id { get; set; }
    }
}
