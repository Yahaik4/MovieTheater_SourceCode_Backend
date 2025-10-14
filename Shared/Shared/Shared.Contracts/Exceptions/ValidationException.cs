using Shared.Contracts.Enums;

namespace Shared.Contracts.Exceptions
{
    public class ValidationException : BaseException
    {
        public ValidationException(string message)
            : base(message, StatusCodeEnum.ValidationError)
        {
        }
    }
}
