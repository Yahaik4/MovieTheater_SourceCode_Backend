using Shared.Contracts.Interfaces;

namespace AuthenticationService.DataTransferObject.Parameter
{
    public class GetEmailParam : IParam
    {
        public Guid UserId { get; set; }
    }
}
