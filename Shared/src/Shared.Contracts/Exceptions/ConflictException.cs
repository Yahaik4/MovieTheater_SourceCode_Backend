using Shared.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Exceptions
{
    public class ConflictException : BaseException
    {
        public ConflictException(string message)
            : base(message, StatusCodeEnum.Conflict)
        {
        }
    }
}
