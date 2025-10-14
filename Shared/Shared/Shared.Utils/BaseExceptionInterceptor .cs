using Grpc.Core;
using Grpc.Core.Interceptors;
using Shared.Contracts.Enums;
using Shared.Contracts.Exceptions;

namespace Shared.Utils
{
    public class BaseExceptionInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (BaseException ex)
            {
                // Truyền status code và message sang RpcException
                var detail = $"{(int)ex.StatusCode}|{ex.Message}";
                throw new RpcException(new Status(StatusCode.Unknown, detail));
            }
            catch (Exception ex)
            {
                var detail = $"{(int)StatusCodeEnum.InternalServerError}|Unexpected error: {ex.Message}";
                throw new RpcException(new Status(StatusCode.Unknown, detail));
            }
        }
    }
}
