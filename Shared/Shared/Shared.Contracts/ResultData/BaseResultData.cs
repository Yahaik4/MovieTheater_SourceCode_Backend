using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace Shared.Contracts.ResultData
{
    public abstract class BaseResultData : IResultData
    {
        public bool Result { get; set; }
        public string Message { get; set; } = string.Empty;
        public StatusCodeEnum StatusCode { get; set; }
    }
}
