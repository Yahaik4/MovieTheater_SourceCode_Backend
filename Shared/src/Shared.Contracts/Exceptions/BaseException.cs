using System;
using Shared.Contracts.Enums;

namespace Shared.Contracts.Exceptions
{
    public abstract class BaseException : Exception
    {
        public StatusCodeEnum StatusCode { get; }

        protected BaseException(string message, StatusCodeEnum statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
