using Grpc.Core;
using Shared.Contracts.Enums;

namespace Shared.Utils
{
    public static class RpcExceptionParser
    {
        public static (StatusCodeEnum statusCode, string message) Parse(RpcException ex)
        {
            if (string.IsNullOrEmpty(ex.Status.Detail))
                return (StatusCodeEnum.InternalServerError, ex.Message);

            var parts = ex.Status.Detail.Split('|', 2);
            if (parts.Length < 2)
                return (StatusCodeEnum.InternalServerError, ex.Message);

            if (int.TryParse(parts[0], out int codeValue)
                && Enum.IsDefined(typeof(StatusCodeEnum), codeValue))
            {
                return ((StatusCodeEnum)codeValue, parts[1]);
            }

            return (StatusCodeEnum.InternalServerError, parts[1]);
        }
    }
}
