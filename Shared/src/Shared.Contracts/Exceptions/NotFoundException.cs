using Shared.Contracts.Enums;

namespace Shared.Contracts.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message)
            : base(message, StatusCodeEnum.NotFound)
        {
        }
    }
}
