using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Shared.Contracts.Exceptions
{
    public class ConflictException : BaseException
    {
        public ConflictException(string message)
            : base(message, StatusCodeEnum.Conflict)
        {
        }
    }
}
