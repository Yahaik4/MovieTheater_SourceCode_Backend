using src.Common;

namespace src.DataTransferObject.ResultData
{
    public abstract class BaseResultData : IResultData
    {
        public bool Result { get; set; }
        public string Message { get; set; } = string.Empty;
        public StatusCodeEnum StatusCode { get; set; }
    }
}
