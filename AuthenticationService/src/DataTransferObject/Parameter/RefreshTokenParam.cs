using Shared.Contracts.Interfaces;

namespace AuthenticationService.DataTransferObject.Parameter
{
    public class RefreshTokenParam : IParam
    {
        public string RefreshToken { get; set; }
    }
}
