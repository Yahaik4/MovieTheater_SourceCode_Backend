using Shared.Contracts.Interfaces;

namespace ProfileService.DataTransferObject.Parameter
{
    public class GetProfileParam : IParam
    {
        public Guid UserId { get; set; }
    }
}
