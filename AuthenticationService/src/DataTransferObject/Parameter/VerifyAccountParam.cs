using Shared.Contracts.Interfaces;

namespace AuthenticationService.DataTransferObject.Parameter
{
    public class VerifyAccountParam : IParam
    {
        public Guid UserId { get; set; }
    }
}

