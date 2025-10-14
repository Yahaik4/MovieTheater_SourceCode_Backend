using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class RefreshTokenParam : IParam
    {
        public string RefreshToken { get; set; }
    }
}
