using Shared.Contracts.Enums;

namespace Shared.Contracts.Exceptions
{
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message)
            : base(message, StatusCodeEnum.Unauthorized)
        {
        }
    }
}
